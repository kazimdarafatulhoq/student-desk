using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Enums
{
    public static class EnumDisplay
    {
        public static string BloodGroupLabel(BloodGroup value) => value switch
        {
            BloodGroup.APositive => "A+",
            BloodGroup.ANegative => "A-",
            BloodGroup.BPositive => "B+",
            BloodGroup.BNegative => "B-",
            BloodGroup.ABPositive => "AB+",
            BloodGroup.ABNegative => "AB-",
            BloodGroup.OPositive => "O+",
            BloodGroup.ONegative => "O-",
            _ => value.ToString()
        };

        public static string PaymentMethodLabel(PaymentMethod value) => value switch
        {
            PaymentMethod.Cash => "Cash",
            PaymentMethod.Bkash => "bKash",
            PaymentMethod.Nagad => "Nagad",
            PaymentMethod.BankTransfer => "Bank Transfer",
            _ => value.ToString()
        };
    }
}
