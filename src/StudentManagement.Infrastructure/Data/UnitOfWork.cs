using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StudentManagement.Application.Interfaces;
using StudentManagement.Infrastructure.Repositories;

namespace StudentManagement.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly StudentManagementDbContext _db;
    private IDbContextTransaction? _tx;

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

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (_tx is not null) return;
        _tx = await _db.Database.BeginTransactionAsync(ct);
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

    public async ValueTask DisposeAsync()
    {
        if (_tx is not null)
            await _tx.DisposeAsync();
        await _db.DisposeAsync();
    }
}
