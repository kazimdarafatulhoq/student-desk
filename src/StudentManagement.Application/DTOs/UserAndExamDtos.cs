using StudentManagement.Domain.Enums;

namespace StudentManagement.Application.DTOs;

public class UserAccountDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public UserRole Role { get; set; }
    public string? CreatedBy { get; set; }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthSession
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string AcademicSession { get; set; } = "2025-2026";
    public DateTime LoginAt { get; set; } = DateTime.Now;
}

public class ExamClearanceDto
{
    public long ClearanceId { get; set; }
    public int StudentId { get; set; }
    public string RegistrationNo { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string RollNumber { get; set; } = string.Empty;
    public int ExamTermId { get; set; }
    public string TermName { get; set; } = string.Empty;
    public decimal DuesAtClearance { get; set; }
    public bool IsCleared { get; set; }
    public bool AdminOverride { get; set; }
    public string? OverrideReason { get; set; }
}

public class ExamTermDto
{
    public int ExamTermId { get; set; }
    public string TermName { get; set; } = string.Empty;
    public string AcademicSession { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? TimetableJson { get; set; }
}

public class AdmitCardDto
{
    public long AdmitCardId { get; set; }
    public string AdmitCardNo { get; set; } = string.Empty;
    public string BarcodeValue { get; set; } = string.Empty;
    public string RegistrationNo { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public string RollNumber { get; set; } = string.Empty;
    public string TermName { get; set; } = string.Empty;
    public string? TimetableJson { get; set; }
    public DateTime IssuedAt { get; set; }
}
