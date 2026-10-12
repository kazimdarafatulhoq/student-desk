using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Security;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
namespace StudentManagement.Application.Services
{
    public class StudentService
    {
        private readonly IUnitOfWork _uow;

        public StudentService(IUnitOfWork uow) => _uow = uow;

        public async Task<StudentDto> AdmitAsync(StudentAdmissionRequest request, CancellationToken ct = default)
        {
            ValidateAdmission(request);

            // Persist via dbo.usp_AdmitStudent (registration + student + opening invoice/ledger).
            var (studentId, _) = await _uow.Students.AdmitViaProcedureAsync(request, ct);

            var student = await _uow.Students.GetByIdAsync(studentId, ct)
                          ?? throw new InvalidOperationException("Student was admitted but could not be reloaded.");

            return await MapAsync(student, ct);
        }

        public async Task<IReadOnlyList<StudentDto>> SearchAsync(StudentSearchFilter filter, CancellationToken ct = default)
        {
            var students = await _uow.Students.SearchAsync(filter, ct);
            var results = new List<StudentDto>();
            foreach (var s in students)
                results.Add(await MapAsync(s, ct));
            return results;
        }

        public async Task<StudentDto?> GetByIdAsync(int studentId, CancellationToken ct = default)
        {
            var student = await _uow.Students.GetByIdAsync(studentId, ct);
            return student is null ? null : await MapAsync(student, ct);
        }

        public async Task<StudentDto?> FindQuickAsync(string query, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(query))
                return null;

            query = query.Trim();
            var student = await _uow.Students.GetByRegistrationNoAsync(query, ct)
                          ?? await _uow.Students.GetByMobileAsync(query, ct);

            return student is null ? null : await MapAsync(student, ct);
        }

        public async Task UpdateAsync(StudentDto dto, string? updatedBy = null, CancellationToken ct = default)
        {
            if (dto.StudentId <= 0)
                throw new ArgumentException("Student id is required.");
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("Full name is required.");
            if (string.IsNullOrWhiteSpace(dto.FatherName))
                throw new ArgumentException("Father's name is required.");
            if (string.IsNullOrWhiteSpace(dto.GuardianPhone))
                throw new ArgumentException("Guardian phone is required.");
            if (dto.ClassId <= 0 || dto.SectionId <= 0)
                throw new ArgumentException("Class and section are required.");
            if (string.IsNullOrWhiteSpace(dto.RollNumber))
                throw new ArgumentException("Roll number is required.");
            if (dto.MonthlyTuitionFee < 0)
                throw new ArgumentException("Tuition fee cannot be negative.");

            // Persist via dbo.usp_UpdateStudent.
            await _uow.Students.UpdateViaProcedureAsync(dto, updatedBy, ct);
        }

        public async Task SetStatusAsync(int studentId, StudentStatus status, string? updatedBy = null, CancellationToken ct = default)
        {
            if (studentId <= 0)
                throw new ArgumentException("Student id is required.");

            var exists = await _uow.Students.GetByIdAsync(studentId, ct);
            if (exists is null)
                throw new InvalidOperationException("Student not found.");

            // Persist via dbo.usp_SetStudentStatus.
            await _uow.Students.SetStatusViaProcedureAsync(studentId, status, updatedBy, ct);
        }

        public Task MarkInactiveAsync(int studentId, string? updatedBy = null, CancellationToken ct = default)
            => SetStatusAsync(studentId, StudentStatus.Inactive, updatedBy, ct);

        public async Task<IReadOnlyList<ClassDto>> GetClassesAsync(CancellationToken ct = default)
        {
            var classes = await _uow.Academics.GetClassesAsync(ct);
            return classes.Select(c => new ClassDto
            {
                ClassId = c.ClassId,
                ClassName = c.ClassName,
                ClassCode = c.ClassCode
            }).ToList();
        }

        public async Task<IReadOnlyList<SectionDto>> GetSectionsAsync(int classId, CancellationToken ct = default)
        {
            var sections = await _uow.Academics.GetSectionsByClassAsync(classId, ct);
            return sections.Select(s => new SectionDto
            {
                SectionId = s.SectionId,
                ClassId = s.ClassId,
                SectionName = s.SectionName
            }).ToList();
        }

        public async Task<IReadOnlyList<StudentDto>> GetLookupAsync(CancellationToken ct = default)
        {
            var students = await _uow.Students.GetLookupAsync(ct);
            return students.Select(s => new StudentDto
            {
                StudentId = s.StudentId,
                RegistrationNo = s.RegistrationNo,
                FullName = s.FullName,
                ClassName = s.Class?.ClassName ?? string.Empty,
                RollNumber = s.RollNumber
            }).ToList();
        }

        private async Task<StudentDto> MapAsync(Student s, CancellationToken ct)
        {
            var summary = await _uow.Ledger.GetSummaryAsync(s.StudentId, ct);
            return new StudentDto
            {
                StudentId = s.StudentId,
                RegistrationNo = s.RegistrationNo,
                FullName = s.FullName,
                FatherName = s.FatherName,
                MotherName = s.MotherName,
                BloodGroup = s.BloodGroup,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
                AgeDisplay = AgeCalculator.Format(s.DateOfBirth),
                GuardianPhone = s.GuardianPhone,
                PresentAddress = s.PresentAddress,
                PermanentAddress = s.PermanentAddress,
                ClassId = s.ClassId,
                ClassName = s.Class?.ClassName ?? string.Empty,
                SectionId = s.SectionId,
                SectionName = s.Section?.SectionName ?? string.Empty,
                RollNumber = s.RollNumber,
                MonthlyTuitionFee = s.MonthlyTuitionFee,
                AdmissionDate = s.AdmissionDate,
                Status = s.Status,
                AcademicSession = s.AcademicSession,
                NetDue = summary.NetDue,
                PhotoPath = s.PhotoPath
            };
        }

        private static void ValidateAdmission(StudentAdmissionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
                throw new ArgumentException("Full name is required.");
            if (string.IsNullOrWhiteSpace(request.FatherName))
                throw new ArgumentException("Father's name is required.");
            if (string.IsNullOrWhiteSpace(request.GuardianPhone))
                throw new ArgumentException("Guardian phone is required.");
            if (request.ClassId <= 0 || request.SectionId <= 0)
                throw new ArgumentException("Class and section are required.");
            if (string.IsNullOrWhiteSpace(request.RollNumber))
                throw new ArgumentException("Roll number is required.");
            if (request.MonthlyTuitionFee < 0)
                throw new ArgumentException("Tuition fee cannot be negative.");
            if (request.DateOfBirth.Date >= DateTime.Today)
                throw new ArgumentException("Date of birth must be in the past.");
        }
    }
}
