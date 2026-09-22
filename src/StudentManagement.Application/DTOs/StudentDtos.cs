using StudentManagement.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
namespace StudentManagement.Application.DTOs
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        public string RegistrationNo { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string MotherName { get; set; } = string.Empty;
        public BloodGroup BloodGroup { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AgeDisplay { get; set; } = string.Empty;
        public string GuardianPhone { get; set; } = string.Empty;
        public string? PresentAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public string RollNumber { get; set; } = string.Empty;
        public decimal MonthlyTuitionFee { get; set; }
        public DateTime AdmissionDate { get; set; }
        public StudentStatus Status { get; set; }
        public string AcademicSession { get; set; } = "2025-2026";
        public decimal NetDue { get; set; }
        public string? PhotoPath { get; set; }
    }

    public class StudentAdmissionRequest
    {
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
        public string AcademicSession { get; set; } = "2025-2026";
        public string? CreatedBy { get; set; }
        public string? PhotoPath { get; set; }
    }

    public class StudentSearchFilter
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? Query { get; set; }
        public StudentStatus? Status { get; set; }
    }
}
