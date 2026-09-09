using StudentManagement.Domain.Common;

namespace StudentManagement.Domain.Entities;

public class ExamClearance : AuditableEntity
{
    public long ClearanceId { get; set; }
    public int StudentId { get; set; }
    public int ExamTermId { get; set; }
    public decimal DuesAtClearance { get; set; }
    public bool IsCleared { get; set; }
    public bool AdminOverride { get; set; }
    public string? OverrideReason { get; set; }
    public int? ClearedByUserId { get; set; }
    public DateTime? ClearedAt { get; set; }
    public string? Remarks { get; set; }

    public Student? Student { get; set; }
    public ExamTerm? ExamTerm { get; set; }
}
