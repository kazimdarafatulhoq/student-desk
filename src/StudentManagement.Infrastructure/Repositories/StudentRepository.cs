using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using StudentManagement.Infrastructure.Data;

namespace StudentManagement.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly StudentManagementDbContext _db;

    public StudentRepository(StudentManagementDbContext db) => _db = db;

    public Task<Student?> GetByIdAsync(int studentId, CancellationToken ct = default)
        => _db.Students.Include(s => s.Class).Include(s => s.Section)
            .FirstOrDefaultAsync(s => s.StudentId == studentId && !s.IsDeleted, ct);

    public Task<Student?> GetByRegistrationNoAsync(string registrationNo, CancellationToken ct = default)
        => _db.Students.Include(s => s.Class).Include(s => s.Section)
            .FirstOrDefaultAsync(s => s.RegistrationNo == registrationNo && !s.IsDeleted, ct);

    public Task<Student?> GetByMobileAsync(string mobile, CancellationToken ct = default)
        => _db.Students.Include(s => s.Class).Include(s => s.Section)
            .FirstOrDefaultAsync(s => s.GuardianPhone == mobile && !s.IsDeleted, ct);

    public async Task<IReadOnlyList<Student>> SearchAsync(StudentSearchFilter filter, CancellationToken ct = default)
    {
        var q = _db.Students.AsNoTracking()
            .Include(s => s.Class)
            .Include(s => s.Section)
            .Where(s => !s.IsDeleted);

        if (filter.ClassId.HasValue)
            q = q.Where(s => s.ClassId == filter.ClassId.Value);
        if (filter.SectionId.HasValue)
            q = q.Where(s => s.SectionId == filter.SectionId.Value);
        if (filter.Status.HasValue)
            q = q.Where(s => s.Status == filter.Status.Value);
        if (!string.IsNullOrWhiteSpace(filter.Query))
        {
            var term = filter.Query.Trim();
            q = q.Where(s =>
                s.FullName.Contains(term) ||
                s.RollNumber.Contains(term) ||
                s.GuardianPhone.Contains(term) ||
                s.RegistrationNo.Contains(term));
        }

        return await q.OrderBy(s => s.ClassId).ThenBy(s => s.RollNumber).Take(500).ToListAsync(ct);
    }

    public async Task<Student> AddAsync(Student student, CancellationToken ct = default)
    {
        _db.Students.Add(student);
        await _db.SaveChangesAsync(ct);
        await _db.Entry(student).Reference(s => s.Class).LoadAsync(ct);
        await _db.Entry(student).Reference(s => s.Section).LoadAsync(ct);
        return student;
    }

    public async Task UpdateAsync(Student student, CancellationToken ct = default)
    {
        _db.Students.Update(student);
        await Task.CompletedTask;
    }

    public async Task<string> GenerateNextRegistrationNoAsync(int year, CancellationToken ct = default)
    {
        var prefix = $"REG-{year}-";
        var last = await _db.Students
            .Where(s => s.RegistrationNo.StartsWith(prefix))
            .OrderByDescending(s => s.RegistrationNo)
            .Select(s => s.RegistrationNo)
            .FirstOrDefaultAsync(ct);

        var next = 1;
        if (!string.IsNullOrEmpty(last))
        {
            var suffix = last[prefix.Length..];
            if (int.TryParse(suffix, out var n))
                next = n + 1;
        }

        return $"{prefix}{next:D4}";
    }

    public async Task<IReadOnlyList<Student>> GetLookupAsync(CancellationToken ct = default)
        => await _db.Students.AsNoTracking()
            .Include(s => s.Class)
            .Where(s => !s.IsDeleted && s.Status == StudentStatus.Active)
            .OrderBy(s => s.FullName)
            .ToListAsync(ct);
}

public class AcademicRepository : IAcademicRepository
{
    private readonly StudentManagementDbContext _db;
    public AcademicRepository(StudentManagementDbContext db) => _db = db;

    public async Task<IReadOnlyList<AcademicClass>> GetClassesAsync(CancellationToken ct = default)
        => await _db.Classes.AsNoTracking().Where(c => c.IsActive && !c.IsDeleted)
            .OrderBy(c => c.SortOrder).ThenBy(c => c.ClassName).ToListAsync(ct);

    public async Task<IReadOnlyList<Section>> GetSectionsByClassAsync(int classId, CancellationToken ct = default)
        => await _db.Sections.AsNoTracking()
            .Where(s => s.ClassId == classId && s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.SectionName).ToListAsync(ct);

    public async Task<IReadOnlyList<Section>> GetAllSectionsAsync(CancellationToken ct = default)
        => await _db.Sections.AsNoTracking().Where(s => s.IsActive && !s.IsDeleted).ToListAsync(ct);
}
