using StudentManagement.Domain.Common;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Domain.Entities;

public class Student : AuditableEntity
{
    public int StudentId { get; set; }
    public string RegistrationNo { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string MotherName { get; set; } = string.Empty;
    public BloodGroup BloodGroup { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string GuardianPhone { get; set; } = string.Empty;
    public string? PresentAddress { get; set; }
    public string? PermanentAddress { get; set; }
    public int ClassId { get; set; }
    public int SectionId { get; set; }
    public string RollNumber { get; set; } = string.Empty;
    public decimal MonthlyTuitionFee { get; set; }
    public DateTime AdmissionDate { get; set; } = DateTime.Today;
    public StudentStatus Status { get; set; } = StudentStatus.Active;
    public string? PhotoPath { get; set; }
    public string AcademicSession { get; set; } = "2025-2026";

    public AcademicClass? Class { get; set; }
    public Section? Section { get; set; }
    public ICollection<StudentLedger> LedgerEntries { get; set; } = new List<StudentLedger>();
    public ICollection<FeeInvoice> Invoices { get; set; } = new List<FeeInvoice>();
    public ICollection<FeeCollection> Collections { get; set; } = new List<FeeCollection>();
    public ICollection<ExamClearance> ExamClearances { get; set; } = new List<ExamClearance>();
}
