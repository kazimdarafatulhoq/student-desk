using StudentManagement.Domain.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace StudentManagement.Domain.Entities
{
    public class FeeCategory : AuditableEntity
    {
        public int FeeCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsRecurring { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }
}
