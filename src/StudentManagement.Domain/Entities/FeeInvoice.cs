using StudentManagement.Domain.Common;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Domain.Entities;

public class FeeInvoice : AuditableEntity
{
    public long InvoiceId { get; set; }
    public int StudentId { get; set; }
    public string InvoiceNo { get; set; } = string.Empty;
    public string FeePeriod { get; set; } = string.Empty;
    public string CategoryName { get; set; } = "Tuition Fee";
    public decimal Amount { get; set; }
    public decimal FineAmount { get; set; }
    public decimal WaiverAmount { get; set; }
    public decimal NetAmount => Amount + FineAmount - WaiverAmount;
    public DateTime InvoiceDate { get; set; } = DateTime.Today;
    public DateTime? DueDate { get; set; }
    public LedgerStatus Status { get; set; } = LedgerStatus.Open;
    public long? LedgerId { get; set; }

    public Student? Student { get; set; }
}
