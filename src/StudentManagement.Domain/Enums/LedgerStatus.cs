using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Enums
{
    public enum LedgerStatus
    {
        Open = 1,
        Paid = 2,
        Partial = 3,
        Waived = 4,
        Cancelled = 5
    }
}
