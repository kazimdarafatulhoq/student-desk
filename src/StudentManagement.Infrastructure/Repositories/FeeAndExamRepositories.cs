using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using StudentManagement.Infrastructure.Data;

namespace StudentManagement.Infrastructure.Repositories;

public class LedgerRepository : ILedgerRepository
{
    private readonly StudentManagementDbContext _db;
    public LedgerRepository(StudentManagementDbContext db) => _db = db;

    public async Task<IReadOnlyList<StudentLedger>> GetByStudentAsync(int studentId, CancellationToken ct = default)
        => await _db.StudentLedger.AsNoTracking()
            .Where(l => l.StudentId == studentId && !l.IsDeleted)
            .OrderBy(l => l.TransactionDate).ThenBy(l => l.LedgerId)
            .ToListAsync(ct);

    public async Task<LedgerSummaryDto> GetSummaryAsync(int studentId, CancellationToken ct = default)
    {
        var student = await _db.Students.AsNoTracking()
            .FirstOrDefaultAsync(s => s.StudentId == studentId, ct);

        var totals = await _db.StudentLedger.AsNoTracking()
            .Where(l => l.StudentId == studentId && !l.IsDeleted)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                Debit = g.Sum(x => x.DebitAmount),
                Credit = g.Sum(x => x.CreditAmount)
            })
            .FirstOrDefaultAsync(ct);

        var debit = totals?.Debit ?? 0m;
        var credit = totals?.Credit ?? 0m;
        var balance = debit - credit;

        return new LedgerSummaryDto
        {
            StudentId = studentId,
            RegistrationNo = student?.RegistrationNo ?? string.Empty,
            FullName = student?.FullName ?? string.Empty,
            TotalInvoiced = debit,
            TotalPaid = credit,
            AdvanceBalance = balance < 0 ? Math.Abs(balance) : 0,
            NetDue = balance > 0 ? balance : 0
        };
    }

    public async Task<IReadOnlyList<LedgerEntryDto>> GetDetailedAsync(int studentId, CancellationToken ct = default)
    {
        var entries = await GetByStudentAsync(studentId, ct);
        decimal running = 0;
        var result = new List<LedgerEntryDto>();
        foreach (var e in entries)
        {
            running += e.DebitAmount - e.CreditAmount;
            result.Add(new LedgerEntryDto
            {
                LedgerId = e.LedgerId,
                TransactionDate = e.TransactionDate,
                VoucherNo = e.VoucherNo,
                Particulars = e.Particulars,
                FeePeriod = e.FeePeriod,
                DebitAmount = e.DebitAmount,
                CreditAmount = e.CreditAmount,
                RunningBalance = running,
                Status = e.Status,
                EntryType = e.EntryType
            });
        }
        return result;
    }

    public async Task<StudentLedger> AddAsync(StudentLedger entry, CancellationToken ct = default)
    {
        _db.StudentLedger.Add(entry);
        await _db.SaveChangesAsync(ct);
        return entry;
    }

    public async Task AddRangeAsync(IEnumerable<StudentLedger> entries, CancellationToken ct = default)
    {
        _db.StudentLedger.AddRange(entries);
        await _db.SaveChangesAsync(ct);
    }
}

public class FeeRepository : IFeeRepository
{
    private readonly StudentManagementDbContext _db;
    public FeeRepository(StudentManagementDbContext db) => _db = db;

    public async Task<FeeInvoice> AddInvoiceAsync(FeeInvoice invoice, CancellationToken ct = default)
    {
        _db.FeeInvoices.Add(invoice);
        await _db.SaveChangesAsync(ct);
        return invoice;
    }

    public async Task<FeeCollection> AddCollectionAsync(FeeCollection collection, CancellationToken ct = default)
    {
        _db.FeePayments.Add(collection);
        await _db.SaveChangesAsync(ct);
        return collection;
    }

    public async Task<string> GenerateReceiptNoAsync(CancellationToken ct = default)
    {
        var prefix = $"RCP-{DateTime.Now:yyyyMM}-";
        var count = await _db.FeePayments.CountAsync(p => p.ReceiptNo.StartsWith(prefix), ct);
        return $"{prefix}{(count + 1):D5}";
    }

    public async Task<string> GenerateInvoiceNoAsync(CancellationToken ct = default)
    {
        var prefix = $"INV-{DateTime.Now:yyyyMM}-";
        var count = await _db.FeeInvoices.CountAsync(i => i.InvoiceNo.StartsWith(prefix), ct);
        return $"{prefix}{(count + 1):D5}";
    }

    public async Task<IReadOnlyList<string>> GetUnpaidPeriodsAsync(int studentId, CancellationToken ct = default)
    {
        var paid = await _db.FeePayments.AsNoTracking()
            .Where(p => p.StudentId == studentId && !p.IsDeleted)
            .Select(p => p.FeePeriods)
            .ToListAsync(ct);

        var paidSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var periods in paid.Where(p => !string.IsNullOrWhiteSpace(p)))
        {
            foreach (var part in periods!.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                paidSet.Add(part);
        }

        var year = DateTime.Now.Year;
        var all = Enumerable.Range(1, 12)
            .Select(m => new DateTime(year, m, 1).ToString("MMM-yyyy"))
            .Where(p => !paidSet.Contains(p))
            .ToList();

        return all;
    }
}

public class UserRepository : IUserRepository
{
    private readonly StudentManagementDbContext _db;
    public UserRepository(StudentManagementDbContext db) => _db = db;

    public Task<UserAccount?> GetByUsernameAsync(string username, CancellationToken ct = default)
        => _db.Users.FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted, ct);

    public Task<UserAccount?> GetByIdAsync(int userId, CancellationToken ct = default)
        => _db.Users.FirstOrDefaultAsync(u => u.UserId == userId && !u.IsDeleted, ct);

    public async Task<IReadOnlyList<UserAccount>> GetAllAsync(CancellationToken ct = default)
        => await _db.Users.AsNoTracking().Where(u => !u.IsDeleted).OrderBy(u => u.Username).ToListAsync(ct);

    public async Task<UserAccount> AddAsync(UserAccount user, CancellationToken ct = default)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        return user;
    }

    public async Task UpdateAsync(UserAccount user, CancellationToken ct = default)
    {
        _db.Users.Update(user);
        await Task.CompletedTask;
    }
}

public class ExamRepository : IExamRepository
{
    private readonly StudentManagementDbContext _db;
    public ExamRepository(StudentManagementDbContext db) => _db = db;

    public async Task<IReadOnlyList<ExamTerm>> GetActiveTermsAsync(CancellationToken ct = default)
        => await _db.ExamTerms.AsNoTracking()
            .Where(t => t.IsActive && !t.IsDeleted)
            .OrderByDescending(t => t.StartDate)
            .ToListAsync(ct);

    public Task<ExamClearance?> GetClearanceAsync(int studentId, int examTermId, CancellationToken ct = default)
        => _db.ExamClearances.FirstOrDefaultAsync(c => c.StudentId == studentId && c.ExamTermId == examTermId && !c.IsDeleted, ct);

    public async Task<ExamClearance> UpsertClearanceAsync(ExamClearance clearance, CancellationToken ct = default)
    {
        var existing = await GetClearanceAsync(clearance.StudentId, clearance.ExamTermId, ct);
        if (existing is null)
        {
            _db.ExamClearances.Add(clearance);
        }
        else
        {
            existing.DuesAtClearance = clearance.DuesAtClearance;
            existing.IsCleared = clearance.IsCleared;
            existing.AdminOverride = clearance.AdminOverride;
            existing.OverrideReason = clearance.OverrideReason;
            existing.ClearedByUserId = clearance.ClearedByUserId;
            existing.ClearedAt = clearance.ClearedAt;
            existing.Remarks = clearance.Remarks;
            existing.UpdatedAt = DateTime.UtcNow;
            clearance = existing;
        }

        await _db.SaveChangesAsync(ct);
        return clearance;
    }

    public async Task<AdmitCard> IssueAdmitCardAsync(AdmitCard card, CancellationToken ct = default)
    {
        var existing = await _db.AdmitCards
            .FirstOrDefaultAsync(a => a.StudentId == card.StudentId && a.ExamTermId == card.ExamTermId && !a.IsDeleted, ct);

        if (existing is not null)
        {
            existing.BarcodeValue = card.BarcodeValue;
            existing.IssuedAt = card.IssuedAt;
            existing.IssuedByUserId = card.IssuedByUserId;
            existing.ClearanceId = card.ClearanceId;
            existing.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return existing;
        }

        _db.AdmitCards.Add(card);
        await _db.SaveChangesAsync(ct);
        return card;
    }

    public async Task<IReadOnlyList<AdmitCard>> GetAdmitCardsAsync(int examTermId, int? classId = null, CancellationToken ct = default)
    {
        var q = _db.AdmitCards.AsNoTracking()
            .Include(a => a.Student)!.ThenInclude(s => s!.Class)
            .Include(a => a.Student)!.ThenInclude(s => s!.Section)
            .Include(a => a.ExamTerm)
            .Where(a => a.ExamTermId == examTermId && !a.IsDeleted);

        if (classId.HasValue)
            q = q.Where(a => a.Student != null && a.Student.ClassId == classId.Value);

        return await q.OrderBy(a => a.Student!.ClassId).ThenBy(a => a.Student!.RollNumber).ToListAsync(ct);
    }
}
