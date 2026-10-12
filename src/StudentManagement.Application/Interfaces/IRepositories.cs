using StudentManagement.Application.DTOs;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
namespace StudentManagement.Application.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(int studentId, CancellationToken ct = default);
        Task<Student?> GetByRegistrationNoAsync(string registrationNo, CancellationToken ct = default);
        Task<Student?> GetByMobileAsync(string mobile, CancellationToken ct = default);
        Task<IReadOnlyList<Student>> SearchAsync(StudentSearchFilter filter, CancellationToken ct = default);
        Task<Student> AddAsync(Student student, CancellationToken ct = default);
        Task UpdateAsync(Student student, CancellationToken ct = default);
        Task<string> GenerateNextRegistrationNoAsync(int year, CancellationToken ct = default);
        Task<IReadOnlyList<Student>> GetLookupAsync(CancellationToken ct = default);

        /// <summary>Admit via dbo.usp_AdmitStudent (student + opening tuition invoice).</summary>
        Task<(int StudentId, string RegistrationNo)> AdmitViaProcedureAsync(StudentAdmissionRequest request, CancellationToken ct = default);

        /// <summary>Edit profile via dbo.usp_UpdateStudent.</summary>
        Task UpdateViaProcedureAsync(StudentDto dto, string? updatedBy, CancellationToken ct = default);

        /// <summary>Mark inactive / change status via dbo.usp_SetStudentStatus.</summary>
        Task SetStatusViaProcedureAsync(int studentId, StudentStatus status, string? updatedBy, CancellationToken ct = default);
    }

    public interface IAcademicRepository
    {
        Task<IReadOnlyList<AcademicClass>> GetClassesAsync(CancellationToken ct = default);
        Task<IReadOnlyList<Section>> GetSectionsByClassAsync(int classId, CancellationToken ct = default);
        Task<IReadOnlyList<Section>> GetAllSectionsAsync(CancellationToken ct = default);
    }

    public interface ILedgerRepository
    {
        Task<IReadOnlyList<StudentLedger>> GetByStudentAsync(int studentId, CancellationToken ct = default);
        Task<LedgerSummaryDto> GetSummaryAsync(int studentId, CancellationToken ct = default);
        Task<IReadOnlyList<LedgerEntryDto>> GetDetailedAsync(int studentId, CancellationToken ct = default);
        Task<StudentLedger> AddAsync(StudentLedger entry, CancellationToken ct = default);
        Task AddRangeAsync(IEnumerable<StudentLedger> entries, CancellationToken ct = default);
    }

    public interface IFeeRepository
    {
        Task<FeeInvoice> AddInvoiceAsync(FeeInvoice invoice, CancellationToken ct = default);
        Task<FeeCollection> AddCollectionAsync(FeeCollection collection, CancellationToken ct = default);
        Task<string> GenerateReceiptNoAsync(CancellationToken ct = default);
        Task<string> GenerateInvoiceNoAsync(CancellationToken ct = default);
        Task<IReadOnlyList<string>> GetUnpaidPeriodsAsync(int studentId, CancellationToken ct = default);

        /// <summary>Post invoice + ledger debit via dbo.usp_PostTuitionInvoice.</summary>
        Task<(string InvoiceNo, long LedgerId)> PostTuitionInvoiceViaProcedureAsync(
            int studentId, string feePeriod, decimal amount, string categoryName,
            decimal fineAmount, int? userId, CancellationToken ct = default);

        /// <summary>Record payment (+ optional waiver) via dbo.usp_RecordFeePayment.</summary>
        Task<(string ReceiptNo, long LedgerId, long CollectionId)> RecordPaymentViaProcedureAsync(
            int studentId, decimal amountPaid, PaymentMethod paymentMethod, string? feePeriods,
            decimal fineCollected, decimal waiverApplied, string? transactionRef, string? remarks,
            int collectedByUserId, CancellationToken ct = default);
    }

    public interface IUserRepository
    {
        Task<UserAccount?> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task<UserAccount?> GetByIdAsync(int userId, CancellationToken ct = default);
        Task<IReadOnlyList<UserAccount>> GetAllAsync(CancellationToken ct = default);
        Task<UserAccount> AddAsync(UserAccount user, CancellationToken ct = default);
        Task UpdateAsync(UserAccount user, CancellationToken ct = default);
    }

    public interface IExamRepository
    {
        Task<IReadOnlyList<ExamTerm>> GetActiveTermsAsync(CancellationToken ct = default);
        Task<ExamClearance?> GetClearanceAsync(int studentId, int examTermId, CancellationToken ct = default);
        Task<ExamClearance> UpsertClearanceAsync(ExamClearance clearance, CancellationToken ct = default);
        Task<AdmitCard> IssueAdmitCardAsync(AdmitCard card, CancellationToken ct = default);
        Task<IReadOnlyList<AdmitCard>> GetAdmitCardsAsync(int examTermId, int? classId = null, CancellationToken ct = default);
    }

    public interface IUnitOfWork
    {
        IStudentRepository Students { get; }
        IAcademicRepository Academics { get; }
        ILedgerRepository Ledger { get; }
        IFeeRepository Fees { get; }
        IUserRepository Users { get; }
        IExamRepository Exams { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
        /// <summary>
        /// Runs work inside a SQL transaction compatible with EnableRetryOnFailure
        /// (SqlServerRetryingExecutionStrategy). Prefer this over Begin/Commit.
        /// </summary>
        Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default);
        Task<bool> TestConnectionAsync(CancellationToken ct = default);
    }
}
