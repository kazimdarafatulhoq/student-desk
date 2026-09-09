using StudentManagement.Domain.Common;

namespace StudentManagement.Domain.Entities;

public class AdmitCard : AuditableEntity
{
    public long AdmitCardId { get; set; }
    public int StudentId { get; set; }
    public int ExamTermId { get; set; }
    public string AdmitCardNo { get; set; } = string.Empty;
    public string BarcodeValue { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; } = DateTime.Now;
    public int IssuedByUserId { get; set; }
    public bool IsPrinted { get; set; }
    public long? ClearanceId { get; set; }

    public Student? Student { get; set; }
    public ExamTerm? ExamTerm { get; set; }
    public ExamClearance? Clearance { get; set; }
}
