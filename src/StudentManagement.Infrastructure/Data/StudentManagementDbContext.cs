using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace StudentManagement.Infrastructure.Data
{
    public class StudentManagementDbContext : DbContext
    {
        public StudentManagementDbContext(DbContextOptions<StudentManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAccount> Users => Set<UserAccount>();
        public DbSet<AcademicClass> Classes => Set<AcademicClass>();
        public DbSet<Section> Sections => Set<Section>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<FeeCategory> FeeCategories => Set<FeeCategory>();
        public DbSet<StudentFeeStructure> StudentFeeStructures => Set<StudentFeeStructure>();
        public DbSet<StudentLedger> StudentLedger => Set<StudentLedger>();
        public DbSet<FeeInvoice> FeeInvoices => Set<FeeInvoice>();
        public DbSet<FeeCollection> FeePayments => Set<FeeCollection>();
        public DbSet<ExamTerm> ExamTerms => Set<ExamTerm>();
        public DbSet<ExamClearance> ExamClearances => Set<ExamClearance>();
        public DbSet<AdmitCard> AdmitCards => Set<AdmitCard>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAccount>(e =>
            {
                e.ToTable("Users");
                e.HasKey(x => x.UserId);
                e.Property(x => x.Username).HasMaxLength(50).IsRequired();
                e.HasIndex(x => x.Username).IsUnique();
                e.Property(x => x.FullName).HasMaxLength(150).IsRequired();
                e.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
                e.Property(x => x.Email).HasMaxLength(150);
                e.Property(x => x.Phone).HasMaxLength(20);
                e.Property(x => x.Role).HasConversion<int>();
            });

            modelBuilder.Entity<AcademicClass>(e =>
            {
                e.ToTable("Classes");
                e.HasKey(x => x.ClassId);
                e.Property(x => x.ClassName).HasMaxLength(100).IsRequired();
                e.Property(x => x.ClassCode).HasMaxLength(20);
            });

            modelBuilder.Entity<Section>(e =>
            {
                e.ToTable("Sections");
                e.HasKey(x => x.SectionId);
                e.Property(x => x.SectionName).HasMaxLength(50).IsRequired();
                e.HasOne(x => x.Class).WithMany(c => c.Sections).HasForeignKey(x => x.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Student>(e =>
            {
                e.ToTable("Students");
                e.HasKey(x => x.StudentId);
                e.Property(x => x.RegistrationNo).HasMaxLength(30).IsRequired();
                e.HasIndex(x => x.RegistrationNo).IsUnique();
                e.HasIndex(x => x.GuardianPhone);
                e.Property(x => x.FullName).HasMaxLength(150).IsRequired();
                e.Property(x => x.FatherName).HasMaxLength(150).IsRequired();
                e.Property(x => x.MotherName).HasMaxLength(150);
                e.Property(x => x.GuardianPhone).HasMaxLength(20).IsRequired();
                e.Property(x => x.PresentAddress).HasMaxLength(500);
                e.Property(x => x.PermanentAddress).HasMaxLength(500);
                e.Property(x => x.RollNumber).HasMaxLength(20).IsRequired();
                e.Property(x => x.MonthlyTuitionFee).HasColumnType("decimal(18,2)");
                e.Property(x => x.BloodGroup).HasConversion<int>();
                e.Property(x => x.Gender).HasConversion<int>();
                e.Property(x => x.Status).HasConversion<int>();
                e.Property(x => x.AcademicSession).HasMaxLength(20);
                e.HasOne(x => x.Class).WithMany(c => c.Students).HasForeignKey(x => x.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Section).WithMany(s => s.Students).HasForeignKey(x => x.SectionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FeeCategory>(e =>
            {
                e.ToTable("FeeCategories");
                e.HasKey(x => x.FeeCategoryId);
                e.Property(x => x.CategoryName).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<StudentFeeStructure>(e =>
            {
                e.ToTable("StudentFeeStructures");
                e.HasKey(x => x.StructureId);
                e.Property(x => x.Amount).HasColumnType("decimal(18,2)");
                e.HasOne(x => x.Student).WithMany().HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.FeeCategory).WithMany().HasForeignKey(x => x.FeeCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StudentLedger>(e =>
            {
                e.ToTable("StudentLedger");
                e.HasKey(x => x.LedgerId);
                e.Property(x => x.VoucherNo).HasMaxLength(50).IsRequired();
                e.Property(x => x.Particulars).HasMaxLength(500).IsRequired();
                e.Property(x => x.FeePeriod).HasMaxLength(100);
                e.Property(x => x.DebitAmount).HasColumnType("decimal(18,2)");
                e.Property(x => x.CreditAmount).HasColumnType("decimal(18,2)");
                e.Property(x => x.ReferenceNo).HasMaxLength(100);
                e.Property(x => x.EntryType).HasConversion<int>();
                e.Property(x => x.Status).HasConversion<int>();
                e.HasOne(x => x.Student).WithMany(s => s.LedgerEntries).HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(x => new { x.StudentId, x.TransactionDate });
            });

            modelBuilder.Entity<FeeInvoice>(e =>
            {
                e.ToTable("FeeInvoices");
                e.HasKey(x => x.InvoiceId);
                e.Property(x => x.InvoiceNo).HasMaxLength(50).IsRequired();
                e.Property(x => x.FeePeriod).HasMaxLength(100);
                e.Property(x => x.CategoryName).HasMaxLength(100);
                e.Property(x => x.Amount).HasColumnType("decimal(18,2)");
                e.Property(x => x.FineAmount).HasColumnType("decimal(18,2)");
                e.Property(x => x.WaiverAmount).HasColumnType("decimal(18,2)");
                e.Ignore(x => x.NetAmount);
                e.Property(x => x.Status).HasConversion<int>();
                e.HasOne(x => x.Student).WithMany(s => s.Invoices).HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FeeCollection>(e =>
            {
                e.ToTable("FeePayments");
                e.HasKey(x => x.CollectionId);
                e.Property(x => x.ReceiptNo).HasMaxLength(50).IsRequired();
                e.HasIndex(x => x.ReceiptNo).IsUnique();
                e.Property(x => x.AmountPaid).HasColumnType("decimal(18,2)");
                e.Property(x => x.FineCollected).HasColumnType("decimal(18,2)");
                e.Property(x => x.WaiverApplied).HasColumnType("decimal(18,2)");
                e.Property(x => x.TransactionRef).HasMaxLength(100);
                e.Property(x => x.FeePeriods).HasMaxLength(250);
                e.Property(x => x.Remarks).HasMaxLength(500);
                e.Property(x => x.PaymentMethod).HasConversion<int>();
                e.HasOne(x => x.Student).WithMany(s => s.Collections).HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ExamTerm>(e =>
            {
                e.ToTable("ExamTerms");
                e.HasKey(x => x.ExamTermId);
                e.Property(x => x.TermName).HasMaxLength(100).IsRequired();
                e.Property(x => x.AcademicSession).HasMaxLength(20);
            });

            modelBuilder.Entity<ExamClearance>(e =>
            {
                e.ToTable("ExamClearances");
                e.HasKey(x => x.ClearanceId);
                e.Property(x => x.DuesAtClearance).HasColumnType("decimal(18,2)");
                e.Property(x => x.OverrideReason).HasMaxLength(500);
                e.HasIndex(x => new { x.StudentId, x.ExamTermId }).IsUnique();
                e.HasOne(x => x.Student).WithMany(s => s.ExamClearances).HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.ExamTerm).WithMany().HasForeignKey(x => x.ExamTermId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AdmitCard>(e =>
            {
                e.ToTable("AdmitCards");
                e.HasKey(x => x.AdmitCardId);
                e.Property(x => x.AdmitCardNo).HasMaxLength(50).IsRequired();
                e.Property(x => x.BarcodeValue).HasMaxLength(100).IsRequired();
                e.HasOne(x => x.Student).WithMany().HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.ExamTerm).WithMany().HasForeignKey(x => x.ExamTermId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Clearance).WithMany().HasForeignKey(x => x.ClearanceId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
