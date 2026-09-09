using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Enums
{
    public enum LedgerEntryType
    {
        Invoice = 1,
        Payment = 2,
        Fine = 3,
        Waiver = 4,
        Adjustment = 5,
        Advance = 6
    }
}
