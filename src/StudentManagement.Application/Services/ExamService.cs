using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
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
    public class ExamService
    {
        private readonly IUnitOfWork _uow;

        public ExamService(IUnitOfWork uow) => _uow = uow;

        public Task<IReadOnlyList<ExamTerm>> GetActiveTermsAsync(CancellationToken ct = default)
            => _uow.Exams.GetActiveTermsAsync(ct);

        public async Task<ExamClearanceDto> VerifyClearanceAsync(
            int studentId,
            int examTermId,
            bool adminOverride = false,
            string? overrideReason = null,
            int? userId = null,
            UserRole? role = null,
            CancellationToken ct = default)
        {
            var student = await _uow.Students.GetByIdAsync(studentId, ct)
                          ?? throw new InvalidOperationException("Student not found.");
            var terms = await _uow.Exams.GetActiveTermsAsync(ct);
            var term = terms.FirstOrDefault(t => t.ExamTermId == examTermId)
                       ?? throw new InvalidOperationException("Exam term not found.");

            var summary = await _uow.Ledger.GetSummaryAsync(studentId, ct);
            var hasDues = summary.NetDue > 0;

            if (hasDues && !adminOverride)
            {
                return new ExamClearanceDto
                {
                    StudentId = student.StudentId,
                    RegistrationNo = student.RegistrationNo,
                    FullName = student.FullName,
                    ClassName = student.Class?.ClassName ?? string.Empty,
                    RollNumber = student.RollNumber,
                    ExamTermId = term.ExamTermId,
                    TermName = term.TermName,
                    DuesAtClearance = summary.NetDue,
                    IsCleared = false,
                    AdminOverride = false
                };
            }

            if (hasDues && adminOverride && role != UserRole.SuperAdmin && role != UserRole.AccountsManager)
                throw new UnauthorizedAccessException("Only Super Admin or Accounts Manager can override dues.");

            if (hasDues && adminOverride && string.IsNullOrWhiteSpace(overrideReason))
                throw new ArgumentException("Override reason is required when dues exist.");

            var clearance = new ExamClearance
            {
                StudentId = studentId,
                ExamTermId = examTermId,
                DuesAtClearance = summary.NetDue,
                IsCleared = true,
                AdminOverride = adminOverride && hasDues,
                OverrideReason = overrideReason,
                ClearedByUserId = userId,
                ClearedAt = DateTime.Now,
                Remarks = hasDues ? "Cleared with admin override" : "Cleared — no outstanding dues"
            };

            clearance = await _uow.Exams.UpsertClearanceAsync(clearance, ct);
            await _uow.SaveChangesAsync(ct);

            return new ExamClearanceDto
            {
                ClearanceId = clearance.ClearanceId,
                StudentId = student.StudentId,
                RegistrationNo = student.RegistrationNo,
                FullName = student.FullName,
                ClassName = student.Class?.ClassName ?? string.Empty,
                RollNumber = student.RollNumber,
                ExamTermId = term.ExamTermId,
                TermName = term.TermName,
                DuesAtClearance = summary.NetDue,
                IsCleared = true,
                AdminOverride = clearance.AdminOverride,
                OverrideReason = clearance.OverrideReason
            };
        }

        public async Task<AdmitCardDto> IssueAdmitCardAsync(
            int studentId,
            int examTermId,
            int issuedByUserId,
            bool adminOverride = false,
            string? overrideReason = null,
            UserRole? role = null,
            CancellationToken ct = default)
        {
            var clearance = await VerifyClearanceAsync(studentId, examTermId, adminOverride, overrideReason, issuedByUserId, role, ct);
            if (!clearance.IsCleared)
                throw new InvalidOperationException($"Cannot issue admit card. Outstanding dues: ৳{clearance.DuesAtClearance:N2}");

            var student = await _uow.Students.GetByIdAsync(studentId, ct)!;
            var term = (await _uow.Exams.GetActiveTermsAsync(ct)).First(t => t.ExamTermId == examTermId);

            var card = new AdmitCard
            {
                StudentId = studentId,
                ExamTermId = examTermId,
                AdmitCardNo = $"AC-{examTermId:D2}-{student!.RegistrationNo}",
                BarcodeValue = $"{student.RegistrationNo}|{examTermId}|{DateTime.Now:yyyyMMdd}",
                IssuedAt = DateTime.Now,
                IssuedByUserId = issuedByUserId,
                ClearanceId = clearance.ClearanceId
            };

            card = await _uow.Exams.IssueAdmitCardAsync(card, ct);
            await _uow.SaveChangesAsync(ct);

            return new AdmitCardDto
            {
                AdmitCardId = card.AdmitCardId,
                AdmitCardNo = card.AdmitCardNo,
                BarcodeValue = card.BarcodeValue,
                RegistrationNo = student.RegistrationNo,
                FullName = student.FullName,
                FatherName = student.FatherName,
                ClassName = student.Class?.ClassName ?? string.Empty,
                SectionName = student.Section?.SectionName ?? string.Empty,
                RollNumber = student.RollNumber,
                TermName = term.TermName,
                TimetableJson = term.TimetableJson,
                IssuedAt = card.IssuedAt,
                PhotoPath = student.PhotoPath,
                Venue = "Main Auditorium & Hall 204",
                CampusAddress = "Dhanmondi Campus, Dhaka-1205",
                FeeClearanceNote = clearance.AdminOverride
                    ? "Fee Clearance Verified: Issued with admin override."
                    : "Fee Clearance Verified: All required tuition accounts verified."
            };
        }

        public async Task<IReadOnlyList<AdmitCardDto>> GetBatchAdmitCardsAsync(int examTermId, int? classId = null, CancellationToken ct = default)
        {
            var cards = await _uow.Exams.GetAdmitCardsAsync(examTermId, classId, ct);
            return cards.Select(c => new AdmitCardDto
            {
                AdmitCardId = c.AdmitCardId,
                AdmitCardNo = c.AdmitCardNo,
                BarcodeValue = c.BarcodeValue,
                RegistrationNo = c.Student?.RegistrationNo ?? string.Empty,
                FullName = c.Student?.FullName ?? string.Empty,
                FatherName = c.Student?.FatherName ?? string.Empty,
                ClassName = c.Student?.Class?.ClassName ?? string.Empty,
                SectionName = c.Student?.Section?.SectionName ?? string.Empty,
                RollNumber = c.Student?.RollNumber ?? string.Empty,
                TermName = c.ExamTerm?.TermName ?? string.Empty,
                TimetableJson = c.ExamTerm?.TimetableJson,
                IssuedAt = c.IssuedAt,
                PhotoPath = c.Student?.PhotoPath,
                Venue = "Main Auditorium & Hall 204",
                CampusAddress = "Dhanmondi Campus, Dhaka-1205",
                FeeClearanceNote = "Fee Clearance Verified: All required tuition accounts verified."
            }).ToList();
        }
    }
}
