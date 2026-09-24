using StudentManagement.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
namespace StudentManagement.Application.DTOs
{
    public class LedgerSummaryDto
    {
        public int StudentId { get; set; }
        public string RegistrationNo { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public decimal TotalInvoiced { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal AdvanceBalance { get; set; }
        public decimal NetDue { get; set; }
    }

    public class LedgerEntryDto
    {
        public long LedgerId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string VoucherNo { get; set; } = string.Empty;
        public string Particulars { get; set; } = string.Empty;
        public string? FeePeriod { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal RunningBalance { get; set; }
        public LedgerStatus Status { get; set; }
        public LedgerEntryType EntryType { get; set; }
    }

    public class FeeCollectionRequest
    {
        public int StudentId { get; set; }
        public List<string> FeePeriods { get; set; } = new();
        public decimal TuitionAmount { get; set; }
        public decimal FineAmount { get; set; }
        public decimal WaiverAmount { get; set; }
        /// <summary>Optional named charges (registration, transport, etc.). Zero amounts are ignored.</summary>
        public List<NamedFeeAmount> OtherFees { get; set; } = new();
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public string? TransactionRef { get; set; }
        public string? Remarks { get; set; }
        public int CollectedByUserId { get; set; }

        public decimal OtherFeesTotal => OtherFees.Where(f => f.Amount > 0).Sum(f => f.Amount);
    }

    public class NamedFeeAmount
    {
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class FeeCollectionResult
    {
        public long CollectionId { get; set; }
        public string ReceiptNo { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string RegistrationNo { get; set; } = string.Empty;
        public string FeePeriods { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
    }

    public class ClassDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string? ClassCode { get; set; }
    }

    public class SectionDto
    {
        public int SectionId { get; set; }
        public int ClassId { get; set; }
        public string SectionName { get; set; } = string.Empty;
    }
}
