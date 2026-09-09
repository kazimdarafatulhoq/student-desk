using StudentManagement.Domain.Common;
using StudentManagement.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace StudentManagement.Domain.Entities
{
    public class UserAccount : AuditableEntity
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public UserRole Role { get; set; } = UserRole.Teacher;
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginAt { get; set; }
    }
}
