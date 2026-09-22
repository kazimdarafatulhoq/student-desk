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

            var year = DateTime.Now.Year;
            var regNo = await _uow.Students.GenerateNextRegistrationNoAsync(year, ct);

            var student = new Student
            {
                RegistrationNo = regNo,
                FullName = request.FullName.Trim(),
                FatherName = request.FatherName.Trim(),
                MotherName = request.MotherName.Trim(),
                BloodGroup = request.BloodGroup,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth.Date,
                GuardianPhone = request.GuardianPhone.Trim(),
                PresentAddress = request.PresentAddress?.Trim(),
                PermanentAddress = request.PermanentAddress?.Trim(),
                ClassId = request.ClassId,
                SectionId = request.SectionId,
                RollNumber = request.RollNumber.Trim(),
                MonthlyTuitionFee = request.MonthlyTuitionFee,
                AdmissionDate = DateTime.Today,
                Status = StudentStatus.Active,
                AcademicSession = request.AcademicSession,
                PhotoPath = request.PhotoPath,
                CreatedBy = request.CreatedBy
            };

            await _uow.BeginTransactionAsync(ct);
            try
            {
                student = await _uow.Students.AddAsync(student, ct);

                var invoiceNo = await _uow.Fees.GenerateInvoiceNoAsync(ct);
                var voucherNo = $"INV-{DateTime.Now:yyyyMMdd}-{student.StudentId:D4}";
                var period = DateTime.Today.ToString("MMM-yyyy");

                var ledger = new StudentLedger
                {
                    StudentId = student.StudentId,
                    TransactionDate = DateTime.Now,
                    VoucherNo = voucherNo,
                    Particulars = $"Admission / Tuition Fee - {period}",
                    FeePeriod = period,
                    DebitAmount = request.MonthlyTuitionFee,
                    CreditAmount = 0,
                    EntryType = LedgerEntryType.Invoice,
                    Status = LedgerStatus.Open,
                    CreatedBy = request.CreatedBy
                };
                ledger = await _uow.Ledger.AddAsync(ledger, ct);

                await _uow.Fees.AddInvoiceAsync(new FeeInvoice
                {
                    StudentId = student.StudentId,
                    InvoiceNo = invoiceNo,
                    FeePeriod = period,
                    CategoryName = "Tuition Fee",
                    Amount = request.MonthlyTuitionFee,
                    InvoiceDate = DateTime.Today,
                    DueDate = DateTime.Today.AddDays(10),
                    Status = LedgerStatus.Open,
                    LedgerId = ledger.LedgerId,
                    CreatedBy = request.CreatedBy
                }, ct);

                await _uow.SaveChangesAsync(ct);
                await _uow.CommitAsync(ct);
            }
            catch
            {
                await _uow.RollbackAsync(ct);
                throw;
            }

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

        public async Task UpdateAsync(StudentDto dto, CancellationToken ct = default)
        {
            var student = await _uow.Students.GetByIdAsync(dto.StudentId, ct)
                          ?? throw new InvalidOperationException("Student not found.");

            student.FullName = dto.FullName.Trim();
            student.FatherName = dto.FatherName.Trim();
            student.MotherName = dto.MotherName.Trim();
            student.BloodGroup = dto.BloodGroup;
            student.Gender = dto.Gender;
            student.DateOfBirth = dto.DateOfBirth.Date;
            student.GuardianPhone = dto.GuardianPhone.Trim();
            student.PresentAddress = dto.PresentAddress;
            student.PermanentAddress = dto.PermanentAddress;
            student.ClassId = dto.ClassId;
            student.SectionId = dto.SectionId;
            student.RollNumber = dto.RollNumber.Trim();
            student.MonthlyTuitionFee = dto.MonthlyTuitionFee;
            student.Status = dto.Status;
            student.UpdatedAt = DateTime.UtcNow;

            await _uow.Students.UpdateAsync(student, ct);
            await _uow.SaveChangesAsync(ct);
        }

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
