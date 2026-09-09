using StudentManagement.Domain.Common;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Domain.Entities;

public class StudentLedger : AuditableEntity
{
    public long LedgerId { get; set; }
    public int StudentId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string VoucherNo { get; set; } = string.Empty;
    public string Particulars { get; set; } = string.Empty;
    public string? FeePeriod { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public LedgerEntryType EntryType { get; set; }
    public LedgerStatus Status { get; set; } = LedgerStatus.Open;
    public string? ReferenceNo { get; set; }
    public int? CreatedByUserId { get; set; }

    public Student? Student { get; set; }
}
