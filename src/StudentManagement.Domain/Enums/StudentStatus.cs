using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Enums
{
    public enum StudentStatus
    {
        Active = 1,
        Inactive = 2,
        Graduated = 3,
        Transferred = 4,
        Suspended = 5
    }
}
