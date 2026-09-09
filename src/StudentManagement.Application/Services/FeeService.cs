using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Application.Services;

public class FeeService
{
    private readonly IUnitOfWork _uow;

    public FeeService(IUnitOfWork uow) => _uow = uow;

    public async Task<FeeCollectionResult> CollectAsync(FeeCollectionRequest request, CancellationToken ct = default)
    {
        if (request.StudentId <= 0)
            throw new ArgumentException("Student is required.");
        if (request.FeePeriods.Count == 0)
            throw new ArgumentException("Select at least one fee period.");

        var netPayable = request.TuitionAmount + request.FineAmount - request.WaiverAmount;
        if (netPayable <= 0)
            throw new ArgumentException("Net payable amount must be greater than zero.");

        var student = await _uow.Students.GetByIdAsync(request.StudentId, ct)
                      ?? throw new InvalidOperationException("Student not found.");

        await _uow.BeginTransactionAsync(ct);
        try
        {
            var receiptNo = await _uow.Fees.GenerateReceiptNoAsync(ct);
            var periods = string.Join(", ", request.FeePeriods);
            var voucherNo = $"RCPT-{DateTime.Now:yyyyMMddHHmmss}";

            if (request.TuitionAmount > 0)
            {
                await _uow.Fees.AddInvoiceAsync(new FeeInvoice
                {
                    StudentId = request.StudentId,
                    InvoiceNo = await _uow.Fees.GenerateInvoiceNoAsync(ct),
                    FeePeriod = periods,
                    CategoryName = "Tuition Fee",
                    Amount = request.TuitionAmount,
                    FineAmount = request.FineAmount,
                    WaiverAmount = request.WaiverAmount,
                    InvoiceDate = DateTime.Today,
                    Status = LedgerStatus.Paid,
                    CreatedBy = request.CollectedByUserId.ToString()
                }, ct);

                await _uow.Ledger.AddAsync(new StudentLedger
                {
                    StudentId = request.StudentId,
                    TransactionDate = DateTime.Now,
                    VoucherNo = $"INV-{voucherNo}",
                    Particulars = $"Tuition Fee Invoice ({periods})",
                    FeePeriod = periods,
                    DebitAmount = request.TuitionAmount + request.FineAmount,
                    CreditAmount = 0,
                    EntryType = request.FineAmount > 0 ? LedgerEntryType.Fine : LedgerEntryType.Invoice,
                    Status = LedgerStatus.Open,
                    CreatedByUserId = request.CollectedByUserId
                }, ct);
            }

            if (request.WaiverAmount > 0)
            {
                await _uow.Ledger.AddAsync(new StudentLedger
                {
                    StudentId = request.StudentId,
                    TransactionDate = DateTime.Now,
                    VoucherNo = $"WVR-{voucherNo}",
                    Particulars = $"Waiver / Discount ({periods})",
                    FeePeriod = periods,
                    DebitAmount = 0,
                    CreditAmount = request.WaiverAmount,
                    EntryType = LedgerEntryType.Waiver,
                    Status = LedgerStatus.Waived,
                    CreatedByUserId = request.CollectedByUserId
                }, ct);
            }

            var paymentLedger = await _uow.Ledger.AddAsync(new StudentLedger
            {
                StudentId = request.StudentId,
                TransactionDate = DateTime.Now,
                VoucherNo = voucherNo,
                Particulars = $"Fee Collection via {request.PaymentMethod} ({periods})",
                FeePeriod = periods,
                DebitAmount = 0,
                CreditAmount = netPayable,
                EntryType = LedgerEntryType.Payment,
                Status = LedgerStatus.Paid,
                ReferenceNo = request.TransactionRef,
                CreatedByUserId = request.CollectedByUserId
            }, ct);

            var collection = await _uow.Fees.AddCollectionAsync(new FeeCollection
            {
                StudentId = request.StudentId,
                ReceiptNo = receiptNo,
                PaymentDate = DateTime.Now,
                AmountPaid = netPayable,
                FineCollected = request.FineAmount,
                WaiverApplied = request.WaiverAmount,
                PaymentMethod = request.PaymentMethod,
                TransactionRef = request.TransactionRef,
                FeePeriods = periods,
                Remarks = request.Remarks,
                LedgerId = paymentLedger.LedgerId,
                CollectedByUserId = request.CollectedByUserId
            }, ct);

            await _uow.SaveChangesAsync(ct);
            await _uow.CommitAsync(ct);

            return new FeeCollectionResult
            {
                CollectionId = collection.CollectionId,
                ReceiptNo = receiptNo,
                AmountPaid = netPayable,
                PaymentDate = collection.PaymentDate,
                StudentName = student.FullName,
                RegistrationNo = student.RegistrationNo,
                FeePeriods = periods,
                PaymentMethod = request.PaymentMethod
            };
        }
        catch
        {
            await _uow.RollbackAsync(ct);
            throw;
        }
    }

    public Task<LedgerSummaryDto> GetSummaryAsync(int studentId, CancellationToken ct = default)
        => _uow.Ledger.GetSummaryAsync(studentId, ct);

    public Task<IReadOnlyList<LedgerEntryDto>> GetLedgerDetailedAsync(int studentId, CancellationToken ct = default)
        => _uow.Ledger.GetDetailedAsync(studentId, ct);

    public static decimal CalculateNetPayable(decimal tuition, decimal fine, decimal waiver)
        => Math.Max(0, tuition + fine - waiver);

    public static IReadOnlyList<string> BuildMonthMatrix(int year, int startMonth = 1)
    {
        var months = new List<string>();
        for (var m = startMonth; m <= 12; m++)
            months.Add(new DateTime(year, m, 1).ToString("MMM-yyyy"));
        return months;
    }
}
