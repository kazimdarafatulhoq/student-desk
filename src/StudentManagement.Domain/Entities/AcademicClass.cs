using StudentManagement.Domain.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace StudentManagement.Domain.Entities
{
    public class AcademicClass : AuditableEntity
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string? ClassCode { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Section> Sections { get; set; } = new List<Section>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
