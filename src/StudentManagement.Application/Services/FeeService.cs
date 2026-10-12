using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
namespace StudentManagement.Application.Services
{
    public class FeeService
    {
        private readonly IUnitOfWork _uow;

        public FeeService(IUnitOfWork uow) => _uow = uow;

        public async Task<FeeCollectionResult> CollectAsync(FeeCollectionRequest request, CancellationToken ct = default)
        {
            if (request.StudentId <= 0)
                throw new ArgumentException("Student is required.");

            var otherFees = (request.OtherFees ?? new List<NamedFeeAmount>())
                .Where(f => f != null && f.Amount > 0 && !string.IsNullOrWhiteSpace(f.Name))
                .ToList();
            var otherTotal = otherFees.Sum(f => f.Amount);

            if (request.FeePeriods.Count == 0 && otherTotal <= 0)
                throw new ArgumentException("Select at least one tuition month or enter an optional fee amount.");

            var netPayable = request.TuitionAmount + request.FineAmount - request.WaiverAmount + otherTotal;
            if (netPayable <= 0)
                throw new ArgumentException("Net payable amount must be greater than zero.");

            var student = await _uow.Students.GetByIdAsync(request.StudentId, ct)
                          ?? throw new InvalidOperationException("Student not found.");

            var periods = request.FeePeriods.Count > 0
                ? string.Join(", ", request.FeePeriods)
                : "Other Charges";

            var otherNote = otherFees.Count > 0
                ? " | " + string.Join(", ", otherFees.Select(f => $"{f.Name}: ৳{f.Amount:N2}"))
                : string.Empty;
            var remarks = string.IsNullOrWhiteSpace(request.Remarks)
                ? otherNote.TrimStart(' ', '|')
                : request.Remarks.Trim() + otherNote;

            // Each SP manages its own transaction (usp_PostTuitionInvoice / usp_RecordFeePayment).
            if (request.TuitionAmount > 0 && request.FeePeriods.Count > 0)
            {
                await _uow.Fees.PostTuitionInvoiceViaProcedureAsync(
                    request.StudentId,
                    periods,
                    request.TuitionAmount,
                    "Tuition Fee",
                    request.FineAmount,
                    request.CollectedByUserId,
                    ct);
            }

            foreach (var fee in otherFees)
            {
                await _uow.Fees.PostTuitionInvoiceViaProcedureAsync(
                    request.StudentId,
                    periods,
                    fee.Amount,
                    fee.Name.Trim(),
                    0,
                    request.CollectedByUserId,
                    ct);
            }

            var (receiptNo, _, collectionId) = await _uow.Fees.RecordPaymentViaProcedureAsync(
                request.StudentId,
                netPayable,
                request.PaymentMethod,
                periods,
                request.FineAmount,
                request.WaiverAmount,
                request.TransactionRef,
                string.IsNullOrWhiteSpace(remarks) ? null : remarks,
                request.CollectedByUserId,
                ct);

            var lineItems = BuildReceiptLines(request, otherFees);

            return new FeeCollectionResult
            {
                CollectionId = collectionId,
                ReceiptNo = receiptNo,
                AmountPaid = netPayable,
                PaymentDate = DateTime.Now,
                StudentName = student.FullName,
                RegistrationNo = student.RegistrationNo,
                FeePeriods = periods,
                PaymentMethod = request.PaymentMethod,
                PaymentMethodDisplay = FormatPaymentMethod(request.PaymentMethod),
                LineItems = lineItems
            };
        }

        /// <summary>Builds preview/PDF rows — only positive amounts, one description per charge.</summary>
        public static List<ReceiptLineItemDto> BuildReceiptLines(FeeCollectionRequest request, List<NamedFeeAmount>? otherFees = null)
        {
            var lines = new List<ReceiptLineItemDto>();
            otherFees ??= (request.OtherFees ?? new List<NamedFeeAmount>())
                .Where(f => f != null && f.Amount > 0 && !string.IsNullOrWhiteSpace(f.Name))
                .ToList();

            if (request.TuitionAmount > 0 && request.FeePeriods.Count > 0)
            {
                var unit = Math.Round(request.TuitionAmount / request.FeePeriods.Count, 2);
                var remainder = request.TuitionAmount - (unit * request.FeePeriods.Count);
                for (var i = 0; i < request.FeePeriods.Count; i++)
                {
                    var amount = unit + (i == request.FeePeriods.Count - 1 ? remainder : 0);
                    if (amount <= 0) continue;
                    lines.Add(new ReceiptLineItemDto
                    {
                        Description = $"Tuition Fee ({FormatFeePeriod(request.FeePeriods[i])})",
                        Amount = amount
                    });
                }
            }

            foreach (var fee in otherFees)
            {
                lines.Add(new ReceiptLineItemDto
                {
                    Description = fee.Name.Trim(),
                    Amount = fee.Amount
                });
            }

            if (request.FineAmount > 0)
            {
                lines.Add(new ReceiptLineItemDto
                {
                    Description = "Late Fine / Penalty",
                    Amount = request.FineAmount
                });
            }

            if (request.WaiverAmount > 0)
            {
                lines.Add(new ReceiptLineItemDto
                {
                    Description = "Waiver / Discount",
                    Amount = request.WaiverAmount,
                    IsCredit = true
                });
            }

            return lines;
        }

        public static string FormatPaymentMethod(PaymentMethod method) => method switch
        {
            PaymentMethod.Cash => "Cash Counter",
            PaymentMethod.Bkash => "bKash",
            PaymentMethod.Nagad => "Nagad",
            PaymentMethod.BankTransfer => "Bank Transfer",
            _ => method.ToString()
        };

        public static string FormatFeePeriod(string period)
        {
            if (string.IsNullOrWhiteSpace(period))
                return period;

            // "Jan-2026" / "MMM-yyyy" → "January 2026"
            if (DateTime.TryParseExact(period.Trim(), "MMM-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out var dt))
                return dt.ToString("MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture);

            return period.Trim();
        }

        public Task<LedgerSummaryDto> GetSummaryAsync(int studentId, CancellationToken ct = default)
            => _uow.Ledger.GetSummaryAsync(studentId, ct);

        public Task<IReadOnlyList<LedgerEntryDto>> GetLedgerDetailedAsync(int studentId, CancellationToken ct = default)
            => _uow.Ledger.GetDetailedAsync(studentId, ct);

        public static decimal CalculateNetPayable(decimal tuition, decimal fine, decimal waiver, decimal otherFees = 0)
            => Math.Max(0, tuition + fine + otherFees - waiver);

        public static IReadOnlyList<string> BuildMonthMatrix(int year, int startMonth = 1)
        {
            var months = new List<string>();
            for (var m = startMonth; m <= 12; m++)
                months.Add(new DateTime(year, m, 1).ToString("MMM-yyyy"));
            return months;
        }
    }
}
