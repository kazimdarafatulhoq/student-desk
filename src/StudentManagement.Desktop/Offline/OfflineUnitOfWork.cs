using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Desktop.Offline
{
    /// <summary>
    /// In-memory demo data so UI/theme work can run without SQL Server.
    /// Flip App:OfflineMode to false when ready to reconnect the backend.
    /// </summary>
    public sealed class OfflineUnitOfWork : IUnitOfWork
    {
        private readonly OfflineStore _store = OfflineStore.Instance;

        public IStudentRepository Students { get; }
        public IAcademicRepository Academics { get; }
        public ILedgerRepository Ledger { get; }
        public IFeeRepository Fees { get; }
        public IUserRepository Users { get; }
        public IExamRepository Exams { get; }

        public OfflineUnitOfWork()
        {
            Students = new OfflineStudentRepository(_store);
            Academics = new OfflineAcademicRepository(_store);
            Ledger = new OfflineLedgerRepository(_store);
            Fees = new OfflineFeeRepository(_store);
            Users = new OfflineUserRepository(_store);
            Exams = new OfflineExamRepository(_store);
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => Task.FromResult(0);
        public Task BeginTransactionAsync(CancellationToken ct = default) => Task.CompletedTask;
        public Task CommitAsync(CancellationToken ct = default) => Task.CompletedTask;
        public Task RollbackAsync(CancellationToken ct = default) => Task.CompletedTask;
        public Task<bool> TestConnectionAsync(CancellationToken ct = default) => Task.FromResult(false);
    }

    internal sealed class OfflineStore
    {
        public static readonly OfflineStore Instance = new OfflineStore();

        public List<UserAccount> Users { get; } = new List<UserAccount>();
        public List<AcademicClass> Classes { get; } = new List<AcademicClass>();
        public List<Section> Sections { get; } = new List<Section>();
        public List<Student> Students { get; } = new List<Student>();
        public List<StudentLedger> Ledger { get; } = new List<StudentLedger>();
        public List<FeeInvoice> Invoices { get; } = new List<FeeInvoice>();
        public List<FeeCollection> Payments { get; } = new List<FeeCollection>();
        public List<ExamTerm> Terms { get; } = new List<ExamTerm>();
        public List<ExamClearance> Clearances { get; } = new List<ExamClearance>();
        public List<AdmitCard> AdmitCards { get; } = new List<AdmitCard>();
        public int NextStudentId = 100;
        public long NextLedgerId = 1000;
        public long NextInvoiceId = 1000;
        public long NextPaymentId = 1000;
        public long NextClearanceId = 100;
        public long NextAdmitId = 100;

        private OfflineStore()
        {
            Users.Add(new UserAccount
            {
                UserId = 1,
                Username = "admin",
                FullName = "System Super Admin",
                PasswordHash = "offline",
                Role = UserRole.SuperAdmin,
                IsActive = true
            });

            for (int i = 6; i <= 10; i++)
            {
                var c = new AcademicClass { ClassId = i - 5, ClassName = "Class " + i, ClassCode = "C" + i, SortOrder = i, IsActive = true };
                Classes.Add(c);
                Sections.Add(new Section { SectionId = (i - 5) * 2 - 1, ClassId = c.ClassId, SectionName = "A", IsActive = true, Class = c });
                Sections.Add(new Section { SectionId = (i - 5) * 2, ClassId = c.ClassId, SectionName = "B", IsActive = true, Class = c });
            }

            Terms.Add(new ExamTerm
            {
                ExamTermId = 1,
                TermName = "Mid-Term 2026",
                AcademicSession = "2025-2026",
                StartDate = new DateTime(2026, 3, 1),
                EndDate = new DateTime(2026, 3, 15),
                TimetableJson = "[{\"subject\":\"Bangla\",\"date\":\"2026-03-01\",\"time\":\"10:00 AM\"},{\"subject\":\"English\",\"date\":\"2026-03-03\",\"time\":\"10:00 AM\"}]",
                IsActive = true
            });

            AddDemoStudent(1, "REG-2026-0001", "Ayesha Rahman", "Md. Karim Rahman", "Nasrin Akter", 1, 1, "01", 2500m, 0m);
            AddDemoStudent(2, "REG-2026-0002", "Rahim Uddin", "Abdul Jalil", "Salma Begum", 2, 3, "05", 2800m, 800m);
            AddDemoStudent(3, "REG-2026-0003", "Nusrat Jahan", "Syed Hasan", "Farhana Syed", 3, 5, "12", 3200m, 0m);
            AddDemoStudent(4, "REG-2026-0004", "Tanvir Ahmed", "Imran Ahmed", "Laila Ahmed", 1, 2, "08", 2500m, 2500m);
            AddDemoStudent(5, "REG-2026-0005", "Meherin Islam", "Nazrul Islam", "Shahana Islam", 2, 4, "03", 2800m, 500m);
            NextStudentId = 6;
        }

        private void AddDemoStudent(int id, string reg, string name, string father, string mother, int classId, int sectionId, string roll, decimal fee, decimal due)
        {
            var cls = Classes.First(c => c.ClassId == classId);
            var sec = Sections.First(s => s.SectionId == sectionId);
            var student = new Student
            {
                StudentId = id,
                RegistrationNo = reg,
                FullName = name,
                FatherName = father,
                MotherName = mother,
                BloodGroup = BloodGroup.OPositive,
                Gender = Gender.Female,
                DateOfBirth = new DateTime(2013, 5, 12),
                GuardianPhone = "0171100000" + id,
                PresentAddress = "Dhaka",
                PermanentAddress = "Dhaka",
                ClassId = classId,
                SectionId = sectionId,
                Class = cls,
                Section = sec,
                RollNumber = roll,
                MonthlyTuitionFee = fee,
                AdmissionDate = new DateTime(2026, 1, 5),
                Status = StudentStatus.Active,
                AcademicSession = "2025-2026"
            };
            Students.Add(student);

            Ledger.Add(new StudentLedger
            {
                LedgerId = NextLedgerId++,
                StudentId = id,
                TransactionDate = new DateTime(2026, 1, 5),
                VoucherNo = "INV-DEMO-" + id,
                Particulars = "Tuition Fee - Jan-2026",
                FeePeriod = "Jan-2026",
                DebitAmount = fee,
                CreditAmount = 0,
                EntryType = LedgerEntryType.Invoice,
                Status = LedgerStatus.Open
            });

            if (due < fee)
            {
                Ledger.Add(new StudentLedger
                {
                    LedgerId = NextLedgerId++,
                    StudentId = id,
                    TransactionDate = new DateTime(2026, 1, 10),
                    VoucherNo = "RCPT-DEMO-" + id,
                    Particulars = "Fee Collection via Cash",
                    FeePeriod = "Jan-2026",
                    DebitAmount = 0,
                    CreditAmount = fee - due,
                    EntryType = LedgerEntryType.Payment,
                    Status = LedgerStatus.Paid
                });
            }
        }
    }

    internal sealed class OfflineStudentRepository : IStudentRepository
    {
        private readonly OfflineStore _s;
        public OfflineStudentRepository(OfflineStore s) => _s = s;

        public Task<Student?> GetByIdAsync(int studentId, CancellationToken ct = default)
            => Task.FromResult(_s.Students.FirstOrDefault(x => x.StudentId == studentId));

        public Task<Student?> GetByRegistrationNoAsync(string registrationNo, CancellationToken ct = default)
            => Task.FromResult(_s.Students.FirstOrDefault(x => x.RegistrationNo.Equals(registrationNo, StringComparison.OrdinalIgnoreCase)));

        public Task<Student?> GetByMobileAsync(string mobile, CancellationToken ct = default)
            => Task.FromResult(_s.Students.FirstOrDefault(x => x.GuardianPhone == mobile));

        public Task<IReadOnlyList<Student>> SearchAsync(StudentSearchFilter filter, CancellationToken ct = default)
        {
            IEnumerable<Student> q = _s.Students;
            if (filter.ClassId.HasValue) q = q.Where(s => s.ClassId == filter.ClassId.Value);
            if (filter.SectionId.HasValue) q = q.Where(s => s.SectionId == filter.SectionId.Value);
            if (filter.Status.HasValue) q = q.Where(s => s.Status == filter.Status.Value);
            if (!string.IsNullOrWhiteSpace(filter.Query))
            {
                string t = filter.Query.Trim();
                q = q.Where(s =>
                    s.FullName.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.RollNumber.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.GuardianPhone.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.RegistrationNo.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return Task.FromResult((IReadOnlyList<Student>)q.ToList());
        }

        public Task<Student> AddAsync(Student student, CancellationToken ct = default)
        {
            student.StudentId = _s.NextStudentId++;
            student.Class = _s.Classes.FirstOrDefault(c => c.ClassId == student.ClassId);
            student.Section = _s.Sections.FirstOrDefault(c => c.SectionId == student.SectionId);
            _s.Students.Add(student);
            return Task.FromResult(student);
        }

        public Task UpdateAsync(Student student, CancellationToken ct = default) => Task.CompletedTask;

        public Task<string> GenerateNextRegistrationNoAsync(int year, CancellationToken ct = default)
        {
            int next = _s.Students.Count + 1;
            return Task.FromResult($"REG-{year}-{next:D4}");
        }

        public Task<IReadOnlyList<Student>> GetLookupAsync(CancellationToken ct = default)
            => Task.FromResult((IReadOnlyList<Student>)_s.Students.Where(s => s.Status == StudentStatus.Active).ToList());
    }

    internal sealed class OfflineAcademicRepository : IAcademicRepository
    {
        private readonly OfflineStore _s;
        public OfflineAcademicRepository(OfflineStore s) => _s = s;
        public Task<IReadOnlyList<AcademicClass>> GetClassesAsync(CancellationToken ct = default)
            => Task.FromResult((IReadOnlyList<AcademicClass>)_s.Classes.ToList());
        public Task<IReadOnlyList<Section>> GetSectionsByClassAsync(int classId, CancellationToken ct = default)
            => Task.FromResult((IReadOnlyList<Section>)_s.Sections.Where(s => s.ClassId == classId).ToList());
        public Task<IReadOnlyList<Section>> GetAllSectionsAsync(CancellationToken ct = default)
            => Task.FromResult((IReadOnlyList<Section>)_s.Sections.ToList());
    }

    internal sealed class OfflineLedgerRepository : ILedgerRepository
    {
        private readonly OfflineStore _s;
        public OfflineLedgerRepository(OfflineStore s) => _s = s;

        public Task<IReadOnlyList<StudentLedger>> GetByStudentAsync(int studentId, CancellationToken ct = default)
            => Task.FromResult((IReadOnlyList<StudentLedger>)_s.Ledger.Where(l => l.StudentId == studentId).OrderBy(l => l.TransactionDate).ThenBy(l => l.LedgerId).ToList());

        public async Task<LedgerSummaryDto> GetSummaryAsync(int studentId, CancellationToken ct = default)
        {
            var entries = await GetByStudentAsync(studentId, ct);
            decimal debit = entries.Sum(e => e.DebitAmount);
            decimal credit = entries.Sum(e => e.CreditAmount);
            decimal bal = debit - credit;
            var student = _s.Students.FirstOrDefault(s => s.StudentId == studentId);
            return new LedgerSummaryDto
            {
                StudentId = studentId,
                RegistrationNo = student?.RegistrationNo ?? string.Empty,
                FullName = student?.FullName ?? string.Empty,
                TotalInvoiced = debit,
                TotalPaid = credit,
                AdvanceBalance = bal < 0 ? Math.Abs(bal) : 0,
                NetDue = bal > 0 ? bal : 0
            };
        }

        public async Task<IReadOnlyList<LedgerEntryDto>> GetDetailedAsync(int studentId, CancellationToken ct = default)
        {
            var entries = await GetByStudentAsync(studentId, ct);
            decimal running = 0;
            var list = new List<LedgerEntryDto>();
            foreach (var e in entries)
            {
                running += e.DebitAmount - e.CreditAmount;
                list.Add(new LedgerEntryDto
                {
                    LedgerId = e.LedgerId,
                    TransactionDate = e.TransactionDate,
                    VoucherNo = e.VoucherNo,
                    Particulars = e.Particulars,
                    FeePeriod = e.FeePeriod,
                    DebitAmount = e.DebitAmount,
                    CreditAmount = e.CreditAmount,
                    RunningBalance = running,
                    Status = e.Status,
                    EntryType = e.EntryType
                });
            }
            return list;
        }

        public Task<StudentLedger> AddAsync(StudentLedger entry, CancellationToken ct = default)
        {
            entry.LedgerId = _s.NextLedgerId++;
            _s.Ledger.Add(entry);
            return Task.FromResult(entry);
        }

        public Task AddRangeAsync(IEnumerable<StudentLedger> entries, CancellationToken ct = default)
        {
            foreach (var e in entries)
            {
                e.LedgerId = _s.NextLedgerId++;
                _s.Ledger.Add(e);
            }
            return Task.CompletedTask;
        }
    }

    internal sealed class OfflineFeeRepository : IFeeRepository
    {
        private readonly OfflineStore _s;
        public OfflineFeeRepository(OfflineStore s) => _s = s;

        public Task<FeeInvoice> AddInvoiceAsync(FeeInvoice invoice, CancellationToken ct = default)
        {
            invoice.InvoiceId = _s.NextInvoiceId++;
            _s.Invoices.Add(invoice);
            return Task.FromResult(invoice);
        }

        public Task<FeeCollection> AddCollectionAsync(FeeCollection collection, CancellationToken ct = default)
        {
            collection.CollectionId = _s.NextPaymentId++;
            _s.Payments.Add(collection);
            return Task.FromResult(collection);
        }

        public Task<string> GenerateReceiptNoAsync(CancellationToken ct = default)
            => Task.FromResult($"RCP-{DateTime.Now:yyyyMM}-{_s.Payments.Count + 1:D5}");

        public Task<string> GenerateInvoiceNoAsync(CancellationToken ct = default)
            => Task.FromResult($"INV-{DateTime.Now:yyyyMM}-{_s.Invoices.Count + 1:D5}");

        public Task<IReadOnlyList<string>> GetUnpaidPeriodsAsync(int studentId, CancellationToken ct = default)
        {
            var months = Enumerable.Range(1, 12).Select(m => new DateTime(DateTime.Now.Year, m, 1).ToString("MMM-yyyy")).ToList();
            return Task.FromResult((IReadOnlyList<string>)months);
        }
    }

    internal sealed class OfflineUserRepository : IUserRepository
    {
        private readonly OfflineStore _s;
        public OfflineUserRepository(OfflineStore s) => _s = s;
        public Task<UserAccount?> GetByUsernameAsync(string username, CancellationToken ct = default)
            => Task.FromResult(_s.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)));
        public Task<UserAccount?> GetByIdAsync(int userId, CancellationToken ct = default)
            => Task.FromResult(_s.Users.FirstOrDefault(u => u.UserId == userId));
        public Task<IReadOnlyList<UserAccount>> GetAllAsync(CancellationToken ct = default)
            => Task.FromResult((IReadOnlyList<UserAccount>)_s.Users.ToList());
        public Task<UserAccount> AddAsync(UserAccount user, CancellationToken ct = default)
        {
            user.UserId = _s.Users.Count + 1;
            user.PasswordHash = "offline";
            _s.Users.Add(user);
            return Task.FromResult(user);
        }
        public Task UpdateAsync(UserAccount user, CancellationToken ct = default) => Task.CompletedTask;
    }

    internal sealed class OfflineExamRepository : IExamRepository
    {
        private readonly OfflineStore _s;
        public OfflineExamRepository(OfflineStore s) => _s = s;
        public Task<IReadOnlyList<ExamTerm>> GetActiveTermsAsync(CancellationToken ct = default)
            => Task.FromResult((IReadOnlyList<ExamTerm>)_s.Terms.Where(t => t.IsActive).ToList());
        public Task<ExamClearance?> GetClearanceAsync(int studentId, int examTermId, CancellationToken ct = default)
            => Task.FromResult(_s.Clearances.FirstOrDefault(c => c.StudentId == studentId && c.ExamTermId == examTermId));
        public Task<ExamClearance> UpsertClearanceAsync(ExamClearance clearance, CancellationToken ct = default)
        {
            var existing = _s.Clearances.FirstOrDefault(c => c.StudentId == clearance.StudentId && c.ExamTermId == clearance.ExamTermId);
            if (existing == null)
            {
                clearance.ClearanceId = _s.NextClearanceId++;
                _s.Clearances.Add(clearance);
                return Task.FromResult(clearance);
            }
            existing.IsCleared = clearance.IsCleared;
            existing.AdminOverride = clearance.AdminOverride;
            existing.OverrideReason = clearance.OverrideReason;
            existing.DuesAtClearance = clearance.DuesAtClearance;
            existing.ClearedAt = clearance.ClearedAt;
            return Task.FromResult(existing);
        }
        public Task<AdmitCard> IssueAdmitCardAsync(AdmitCard card, CancellationToken ct = default)
        {
            card.AdmitCardId = _s.NextAdmitId++;
            _s.AdmitCards.Add(card);
            return Task.FromResult(card);
        }
        public Task<IReadOnlyList<AdmitCard>> GetAdmitCardsAsync(int examTermId, int? classId = null, CancellationToken ct = default)
            => Task.FromResult((IReadOnlyList<AdmitCard>)_s.AdmitCards.Where(a => a.ExamTermId == examTermId).ToList());
    }
}
