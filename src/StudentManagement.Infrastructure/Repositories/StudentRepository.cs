using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using StudentManagement.Infrastructure.Data;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace StudentManagement.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentManagementDbContext _db;

        public StudentRepository(StudentManagementDbContext db) => _db = db;

        private static object DbNullIfEmpty(string? value)
            => string.IsNullOrWhiteSpace(value) ? DBNull.Value : value.Trim();

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

        public async Task<(int StudentId, string RegistrationNo)> AdmitViaProcedureAsync(
            StudentAdmissionRequest request, CancellationToken ct = default)
        {
            var pStudentId = new SqlParameter("@StudentId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var pRegistrationNo = new SqlParameter("@RegistrationNo", SqlDbType.NVarChar, 30)
            {
                Direction = ParameterDirection.Output
            };

            await _db.Database.ExecuteSqlRawAsync(@"
EXEC dbo.usp_AdmitStudent
    @FullName = @FullName,
    @FatherName = @FatherName,
    @MotherName = @MotherName,
    @BloodGroup = @BloodGroup,
    @Gender = @Gender,
    @DateOfBirth = @DateOfBirth,
    @GuardianPhone = @GuardianPhone,
    @PresentAddress = @PresentAddress,
    @PermanentAddress = @PermanentAddress,
    @ClassId = @ClassId,
    @SectionId = @SectionId,
    @RollNumber = @RollNumber,
    @MonthlyTuitionFee = @MonthlyTuitionFee,
    @AcademicSession = @AcademicSession,
    @PhotoPath = @PhotoPath,
    @CreatedBy = @CreatedBy,
    @StudentId = @StudentId OUTPUT,
    @RegistrationNo = @RegistrationNo OUTPUT",
                new SqlParameter("@FullName", request.FullName.Trim()),
                new SqlParameter("@FatherName", request.FatherName.Trim()),
                new SqlParameter("@MotherName", DbNullIfEmpty(request.MotherName)),
                new SqlParameter("@BloodGroup", (int)request.BloodGroup),
                new SqlParameter("@Gender", (int)request.Gender),
                new SqlParameter("@DateOfBirth", request.DateOfBirth.Date),
                new SqlParameter("@GuardianPhone", request.GuardianPhone.Trim()),
                new SqlParameter("@PresentAddress", DbNullIfEmpty(request.PresentAddress)),
                new SqlParameter("@PermanentAddress", DbNullIfEmpty(request.PermanentAddress)),
                new SqlParameter("@ClassId", request.ClassId),
                new SqlParameter("@SectionId", request.SectionId),
                new SqlParameter("@RollNumber", request.RollNumber.Trim()),
                new SqlParameter("@MonthlyTuitionFee", request.MonthlyTuitionFee),
                new SqlParameter("@AcademicSession",
                    string.IsNullOrWhiteSpace(request.AcademicSession) ? "2025-2026" : request.AcademicSession.Trim()),
                new SqlParameter("@PhotoPath", DbNullIfEmpty(request.PhotoPath)),
                new SqlParameter("@CreatedBy", DbNullIfEmpty(request.CreatedBy)),
                pStudentId,
                pRegistrationNo);

            var studentId = Convert.ToInt32(pStudentId.Value);
            var registrationNo = Convert.ToString(pRegistrationNo.Value) ?? string.Empty;
            if (studentId <= 0 || string.IsNullOrWhiteSpace(registrationNo))
                throw new InvalidOperationException("usp_AdmitStudent did not return student identifiers.");

            return (studentId, registrationNo);
        }

        public async Task UpdateViaProcedureAsync(StudentDto dto, string? updatedBy, CancellationToken ct = default)
        {
            await _db.Database.ExecuteSqlRawAsync(@"
EXEC dbo.usp_UpdateStudent
    @StudentId = @StudentId,
    @FullName = @FullName,
    @FatherName = @FatherName,
    @MotherName = @MotherName,
    @BloodGroup = @BloodGroup,
    @Gender = @Gender,
    @DateOfBirth = @DateOfBirth,
    @GuardianPhone = @GuardianPhone,
    @PresentAddress = @PresentAddress,
    @PermanentAddress = @PermanentAddress,
    @ClassId = @ClassId,
    @SectionId = @SectionId,
    @RollNumber = @RollNumber,
    @MonthlyTuitionFee = @MonthlyTuitionFee,
    @Status = @Status,
    @PhotoPath = @PhotoPath,
    @AcademicSession = @AcademicSession,
    @UpdatedBy = @UpdatedBy",
                new SqlParameter("@StudentId", dto.StudentId),
                new SqlParameter("@FullName", dto.FullName.Trim()),
                new SqlParameter("@FatherName", dto.FatherName.Trim()),
                new SqlParameter("@MotherName", DbNullIfEmpty(dto.MotherName)),
                new SqlParameter("@BloodGroup", (int)dto.BloodGroup),
                new SqlParameter("@Gender", (int)dto.Gender),
                new SqlParameter("@DateOfBirth", dto.DateOfBirth.Date),
                new SqlParameter("@GuardianPhone", dto.GuardianPhone.Trim()),
                new SqlParameter("@PresentAddress", DbNullIfEmpty(dto.PresentAddress)),
                new SqlParameter("@PermanentAddress", DbNullIfEmpty(dto.PermanentAddress)),
                new SqlParameter("@ClassId", dto.ClassId),
                new SqlParameter("@SectionId", dto.SectionId),
                new SqlParameter("@RollNumber", dto.RollNumber.Trim()),
                new SqlParameter("@MonthlyTuitionFee", dto.MonthlyTuitionFee),
                new SqlParameter("@Status", (int)dto.Status),
                new SqlParameter("@PhotoPath", DbNullIfEmpty(dto.PhotoPath)),
                new SqlParameter("@AcademicSession", DbNullIfEmpty(dto.AcademicSession)),
                new SqlParameter("@UpdatedBy", DbNullIfEmpty(updatedBy)));

            // Drop tracked instances so later GetByIdAsync reloads SP results.
            foreach (var entry in _db.ChangeTracker.Entries<Student>()
                         .Where(e => e.Entity.StudentId == dto.StudentId).ToList())
                entry.State = EntityState.Detached;
        }

        public async Task SetStatusViaProcedureAsync(
            int studentId, StudentStatus status, string? updatedBy, CancellationToken ct = default)
        {
            await _db.Database.ExecuteSqlRawAsync(@"
EXEC dbo.usp_SetStudentStatus
    @StudentId = @StudentId,
    @Status = @Status,
    @UpdatedBy = @UpdatedBy",
                new SqlParameter("@StudentId", studentId),
                new SqlParameter("@Status", (int)status),
                new SqlParameter("@UpdatedBy", DbNullIfEmpty(updatedBy)));

            foreach (var entry in _db.ChangeTracker.Entries<Student>()
                         .Where(e => e.Entity.StudentId == studentId).ToList())
                entry.State = EntityState.Detached;
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
                .Include(s => s.Section)
                .Where(s => !s.IsDeleted && s.Status == StudentStatus.Active)
                .OrderBy(s => s.ClassId)
                .ThenBy(s => s.RollNumber)
                .ThenBy(s => s.FullName)
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
}
