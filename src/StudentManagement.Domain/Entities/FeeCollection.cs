using StudentManagement.Domain.Common;
using StudentManagement.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace StudentManagement.Domain.Entities
{
    public class FeeCollection : AuditableEntity
    {
        public long CollectionId { get; set; }
        public int StudentId { get; set; }
        public string ReceiptNo { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public decimal AmountPaid { get; set; }
        public decimal FineCollected { get; set; }
        public decimal WaiverApplied { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public string? TransactionRef { get; set; }
        public string? FeePeriods { get; set; }
        public string? Remarks { get; set; }
        public long? LedgerId { get; set; }
        public int CollectedByUserId { get; set; }
        public bool IsPrinted { get; set; }

        public Student? Student { get; set; }
    }
}
