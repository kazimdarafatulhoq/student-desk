using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StudentManagement.Application.Interfaces;
using StudentManagement.Infrastructure.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentManagement.Infrastructure.Data
{
    /// <summary>
    /// Implements both IDisposable and IAsyncDisposable so DI scope disposal
    /// on form close / sign-out (sync) does not throw.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly StudentManagementDbContext _db;
        private IDbContextTransaction? _tx;
        private bool _disposed;

        public UnitOfWork(StudentManagementDbContext db)
        {
            _db = db;
            Students = new StudentRepository(_db);
            Academics = new AcademicRepository(_db);
            Ledger = new LedgerRepository(_db);
            Fees = new FeeRepository(_db);
            Users = new UserRepository(_db);
            Exams = new ExamRepository(_db);
        }

        public IStudentRepository Students { get; }
        public IAcademicRepository Academics { get; }
        public ILedgerRepository Ledger { get; }
        public IFeeRepository Fees { get; }
        public IUserRepository Users { get; }
        public IExamRepository Exams { get; }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);

        /// <summary>
        /// Required when SqlServerRetryingExecutionStrategy (EnableRetryOnFailure) is active.
        /// Begins, runs <paramref name="operation"/>, commits — all inside one retriable unit.
        /// </summary>
        public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default)
        {
            if (operation is null)
                throw new ArgumentNullException(nameof(operation));

            var strategy = _db.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _db.Database.BeginTransactionAsync(ct);
                _tx = tx;
                try
                {
                    await operation(ct);
                    await tx.CommitAsync(ct);
                }
                catch
                {
                    try { await tx.RollbackAsync(ct); }
                    catch { /* ignore secondary rollback errors */ }
                    throw;
                }
                finally
                {
                    _tx = null;
                }
            });
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_tx is not null) return;

            // Prefer ExecuteInTransactionAsync when EnableRetryOnFailure is configured.
            // Direct BeginTransaction throws SqlServerRetryingExecutionStrategy errors.
            var strategy = _db.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                if (_tx is null)
                    _tx = await _db.Database.BeginTransactionAsync(ct);
            });
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            if (_tx is null) return;
            await _tx.CommitAsync(ct);
            await _tx.DisposeAsync();
            _tx = null;
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_tx is null) return;
            await _tx.RollbackAsync(ct);
            await _tx.DisposeAsync();
            _tx = null;
        }

        public async Task<bool> TestConnectionAsync(CancellationToken ct = default)
        {
            try
            {
                return await _db.Database.CanConnectAsync(ct);
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try { _tx?.Dispose(); }
            catch { /* ignore */ }
            _tx = null;

            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;
            _disposed = true;

            if (_tx is not null)
            {
                try { await _tx.DisposeAsync(); }
                catch { /* ignore */ }
                _tx = null;
            }

            await _db.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
