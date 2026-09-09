using StudentManagement.Domain.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace StudentManagement.Domain.Entities
{
    public class ExamTerm : AuditableEntity
    {
        public int ExamTermId { get; set; }
        public string TermName { get; set; } = string.Empty;
        public string AcademicSession { get; set; } = "2025-2026";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? TimetableJson { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
