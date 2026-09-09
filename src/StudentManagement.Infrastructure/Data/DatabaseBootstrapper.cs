using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Security;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using StudentManagement.Infrastructure.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace StudentManagement.Infrastructure.Data
{
    /// <summary>
    /// Ensures schema exists and seeds a known admin user when the database is empty.
    /// Prefer running database/StudentManagementDB.sql for full production setup.
    /// </summary>
    public static class DatabaseBootstrapper
    {
        public static async Task InitializeAsync(StudentManagementDbContext db, CancellationToken ct = default)
        {
            await db.Database.EnsureCreatedAsync(ct);

            await EnsureUserAsync(db, "admin", "System Super Admin", "Admin@123", UserRole.SuperAdmin, "admin@school.edu", ct);
            await EnsureUserAsync(db, "accounts", "Accounts Manager", "Accounts@123", UserRole.AccountsManager, "accounts@school.edu", ct);
            await EnsureUserAsync(db, "admission", "Admission Officer", "Admission@123", UserRole.AdmissionOfficer, "admission@school.edu", ct);
            await EnsureUserAsync(db, "teacher", "Class Teacher", "Teacher@123", UserRole.Teacher, "teacher@school.edu", ct);

            if (!await db.Classes.AnyAsync(ct))
            {
                var classes = new[]
                {
                    new AcademicClass { ClassName = "Class 6", ClassCode = "C6", SortOrder = 6 },
                    new AcademicClass { ClassName = "Class 7", ClassCode = "C7", SortOrder = 7 },
                    new AcademicClass { ClassName = "Class 8", ClassCode = "C8", SortOrder = 8 },
                    new AcademicClass { ClassName = "Class 9", ClassCode = "C9", SortOrder = 9 },
                    new AcademicClass { ClassName = "Class 10", ClassCode = "C10", SortOrder = 10 }
                };
                db.Classes.AddRange(classes);
                await db.SaveChangesAsync(ct);

                foreach (var c in classes)
                {
                    db.Sections.Add(new Section { ClassId = c.ClassId, SectionName = "A" });
                    db.Sections.Add(new Section { ClassId = c.ClassId, SectionName = "B" });
                }
            }

            if (!await db.FeeCategories.AnyAsync(ct))
            {
                db.FeeCategories.AddRange(
                    new FeeCategory { CategoryName = "Tuition Fee", IsRecurring = true },
                    new FeeCategory { CategoryName = "Admission Fee", IsRecurring = false },
                    new FeeCategory { CategoryName = "Exam Fee", IsRecurring = false });
            }

            if (!await db.ExamTerms.AnyAsync(ct))
            {
                db.ExamTerms.Add(new ExamTerm
                {
                    TermName = "Mid-Term 2026",
                    AcademicSession = "2025-2026",
                    StartDate = new DateTime(2026, 3, 1),
                    EndDate = new DateTime(2026, 3, 15),
                    TimetableJson = "[{\"subject\":\"Bangla\",\"date\":\"2026-03-01\",\"time\":\"10:00 AM\"},{\"subject\":\"English\",\"date\":\"2026-03-03\",\"time\":\"10:00 AM\"},{\"subject\":\"Mathematics\",\"date\":\"2026-03-05\",\"time\":\"10:00 AM\"}]",
                    IsActive = true
                });
            }

            await db.SaveChangesAsync(ct);
        }

        private static async Task EnsureUserAsync(
            StudentManagementDbContext db,
            string username,
            string fullName,
            string password,
            UserRole role,
            string email,
            CancellationToken ct)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username, ct);
            if (user is null)
            {
                db.Users.Add(new UserAccount
                {
                    Username = username,
                    FullName = fullName,
                    PasswordHash = PasswordHasher.Hash(password),
                    Email = email,
                    Role = role,
                    IsActive = true,
                    CreatedBy = "bootstrap"
                });
                return;
            }

            // Re-hash demo accounts so SQL placeholder seeds still authenticate.
            if (!PasswordHasher.Verify(password, user.PasswordHash))
            {
                user.PasswordHash = PasswordHasher.Hash(password);
                user.UpdatedAt = DateTime.UtcNow;
                user.UpdatedBy = "bootstrap";
            }
        }
    }
}
