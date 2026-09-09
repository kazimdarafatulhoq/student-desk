using StudentManagement.Domain.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace StudentManagement.Domain.Entities
{
    public class StudentFeeStructure : AuditableEntity
    {
        public int StructureId { get; set; }
        public int StudentId { get; set; }
        public int FeeCategoryId { get; set; }
        public decimal Amount { get; set; }
        public string AcademicSession { get; set; } = "2025-2026";
        public bool IsActive { get; set; } = true;

        public Student? Student { get; set; }
        public FeeCategory? FeeCategory { get; set; }
    }
}
