using StudentManagement.Domain.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace StudentManagement.Domain.Entities
{
    public class Section : AuditableEntity
    {
        public int SectionId { get; set; }
        public int ClassId { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public int Capacity { get; set; } = 40;
        public bool IsActive { get; set; } = true;

        public AcademicClass? Class { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
