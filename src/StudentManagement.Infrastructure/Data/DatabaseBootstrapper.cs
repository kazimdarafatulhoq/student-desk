using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Security;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Infrastructure.Data
{
    /// <summary>
    /// Seeds demo users/lookups. Prefer running database/StudentManagementSDB.sql once as an admin.
    /// Does not require CREATE DATABASE permission when the database already exists.
    /// </summary>
    public static class DatabaseBootstrapper
    {
        public static async Task InitializeAsync(StudentManagementDbContext db, CancellationToken ct = default)
        {
            await EnsureSchemaAsync(db, ct);

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
                    new FeeCategory { CategoryName = "Tuition Fee", Description = "Monthly tuition", IsRecurring = true },
                    new FeeCategory { CategoryName = "Registration Fee", Description = "One-time / annual registration", IsRecurring = false },
                    new FeeCategory { CategoryName = "New Admission / Re-admission", Description = "Admission or re-admission charge", IsRecurring = false },
                    new FeeCategory { CategoryName = "Monthly Transport Fee", Description = "School transport / bus", IsRecurring = true },
                    new FeeCategory { CategoryName = "Examination Fee (1st / 2nd Term / Annual / Test)", Description = "Term and test examination fees", IsRecurring = false },
                    new FeeCategory { CategoryName = "Transcript / Testimonial / Certificate Fee", Description = "Document fees", IsRecurring = false },
                    new FeeCategory { CategoryName = "Transfer Certificate / Certification Letter", Description = "TC and certification letters", IsRecurring = false },
                    new FeeCategory { CategoryName = "Hostel Food Charges", Description = "Hostel boarding / food", IsRecurring = true },
                    new FeeCategory { CategoryName = "Miscellaneous", Description = "Other charges", IsRecurring = false });
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

        private static async Task EnsureSchemaAsync(StudentManagementDbContext db, CancellationToken ct)
        {
            var canConnect = false;
            try
            {
                canConnect = await db.Database.CanConnectAsync(ct);
            }
            catch (Exception ex)
            {
                throw BuildSetupException(
                    "Cannot reach SQL Server with the current connection string.",
                    ex);
            }

            if (canConnect)
            {
                // Database already exists — create missing tables only (no CREATE DATABASE).
                await db.Database.EnsureCreatedAsync(ct);
                return;
            }

            // Database missing: try auto-create; if denied, tell the user to run the SQL script.
            try
            {
                await db.Database.EnsureCreatedAsync(ct);
            }
            catch (Exception ex) when (IsCreateDatabaseDenied(ex))
            {
                throw BuildSetupException(
                    "SQL Server refused CREATE DATABASE (permission denied on 'master').\n\n" +
                    "One-time fix:\n" +
                    "1. Open SQL Server Management Studio as Administrator (or use the 'sa' login).\n" +
                    "2. Execute: database\\CreateDatabaseAndGrantAccess.sql\n" +
                    "3. Execute: database\\StudentManagementSDB.sql\n" +
                    "4. Confirm appsettings.json Initial Catalog=StudentManagementSDB.\n" +
                    "5. Restart the application.",
                    ex);
            }
            catch (Exception ex)
            {
                throw BuildSetupException(
                    "Database could not be created or opened. Check SQL Server is running and the connection string is correct.",
                    ex);
            }
        }

        private static bool IsCreateDatabaseDenied(Exception ex)
        {
            for (var current = ex; current != null; current = current.InnerException)
            {
                if (current is SqlException sql &&
                    (sql.Number == 262 || sql.Message.IndexOf("CREATE DATABASE permission denied", StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    return true;
                }

                if (current.Message.IndexOf("CREATE DATABASE permission denied", StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }

        private static InvalidOperationException BuildSetupException(string message, Exception inner)
            => new InvalidOperationException(message + "\n\nDetails: " + GetRootMessage(inner), inner);

        private static string GetRootMessage(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex.Message;
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
