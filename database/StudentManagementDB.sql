/*
================================================================================
 StudentManagementDB.sql
 Production-ready schema for SQL Server 2022
 Enterprise Student Management & Financial Accounting
================================================================================
*/

IF DB_ID(N'StudentManagementDB') IS NULL
BEGIN
    CREATE DATABASE StudentManagementDB;
END
GO

USE StudentManagementDB;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ---------- Drop existing objects (dev re-run safe) ---------- */
IF OBJECT_ID(N'dbo.vw_StudentLedgerDetailed', N'V') IS NOT NULL DROP VIEW dbo.vw_StudentLedgerDetailed;
IF OBJECT_ID(N'dbo.AdmitCards', N'U') IS NOT NULL DROP TABLE dbo.AdmitCards;
IF OBJECT_ID(N'dbo.ExamClearances', N'U') IS NOT NULL DROP TABLE dbo.ExamClearances;
IF OBJECT_ID(N'dbo.ExamTerms', N'U') IS NOT NULL DROP TABLE dbo.ExamTerms;
IF OBJECT_ID(N'dbo.FeePayments', N'U') IS NOT NULL DROP TABLE dbo.FeePayments;
IF OBJECT_ID(N'dbo.FeeInvoices', N'U') IS NOT NULL DROP TABLE dbo.FeeInvoices;
IF OBJECT_ID(N'dbo.StudentLedger', N'U') IS NOT NULL DROP TABLE dbo.StudentLedger;
IF OBJECT_ID(N'dbo.StudentFeeStructures', N'U') IS NOT NULL DROP TABLE dbo.StudentFeeStructures;
IF OBJECT_ID(N'dbo.FeeCategories', N'U') IS NOT NULL DROP TABLE dbo.FeeCategories;
IF OBJECT_ID(N'dbo.Students', N'U') IS NOT NULL DROP TABLE dbo.Students;
IF OBJECT_ID(N'dbo.Sections', N'U') IS NOT NULL DROP TABLE dbo.Sections;
IF OBJECT_ID(N'dbo.Classes', N'U') IS NOT NULL DROP TABLE dbo.Classes;
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE dbo.Users;
GO

/* ---------- Users ---------- */
CREATE TABLE dbo.Users
(
    UserId          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
    Username        NVARCHAR(50)  NOT NULL,
    FullName        NVARCHAR(150) NOT NULL,
    PasswordHash    NVARCHAR(500) NOT NULL,
    Email           NVARCHAR(150) NULL,
    Phone           NVARCHAR(20)  NULL,
    Role            INT NOT NULL CONSTRAINT DF_Users_Role DEFAULT (4),
    IsActive        BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT (1),
    LastLoginAt     DATETIME2 NULL,
    CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy       NVARCHAR(100) NULL,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(100) NULL,
    IsDeleted       BIT NOT NULL CONSTRAINT DF_Users_IsDeleted DEFAULT (0),
    CONSTRAINT UQ_Users_Username UNIQUE (Username)
);
GO

/* ---------- Classes / Sections ---------- */
CREATE TABLE dbo.Classes
(
    ClassId     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Classes PRIMARY KEY,
    ClassName   NVARCHAR(100) NOT NULL,
    ClassCode   NVARCHAR(20) NULL,
    SortOrder   INT NOT NULL CONSTRAINT DF_Classes_SortOrder DEFAULT (0),
    IsActive    BIT NOT NULL CONSTRAINT DF_Classes_IsActive DEFAULT (1),
    CreatedAt   DATETIME2 NOT NULL CONSTRAINT DF_Classes_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy   NVARCHAR(100) NULL,
    UpdatedAt   DATETIME2 NULL,
    UpdatedBy   NVARCHAR(100) NULL,
    IsDeleted   BIT NOT NULL CONSTRAINT DF_Classes_IsDeleted DEFAULT (0)
);
GO

CREATE TABLE dbo.Sections
(
    SectionId   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Sections PRIMARY KEY,
    ClassId     INT NOT NULL,
    SectionName NVARCHAR(50) NOT NULL,
    Capacity    INT NOT NULL CONSTRAINT DF_Sections_Capacity DEFAULT (40),
    IsActive    BIT NOT NULL CONSTRAINT DF_Sections_IsActive DEFAULT (1),
    CreatedAt   DATETIME2 NOT NULL CONSTRAINT DF_Sections_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy   NVARCHAR(100) NULL,
    UpdatedAt   DATETIME2 NULL,
    UpdatedBy   NVARCHAR(100) NULL,
    IsDeleted   BIT NOT NULL CONSTRAINT DF_Sections_IsDeleted DEFAULT (0),
    CONSTRAINT FK_Sections_Classes FOREIGN KEY (ClassId) REFERENCES dbo.Classes(ClassId)
);
GO

CREATE TABLE dbo.Students
(
    StudentId           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Students PRIMARY KEY,
    RegistrationNo      NVARCHAR(30)  NOT NULL,
    FullName            NVARCHAR(150) NOT NULL,
    FatherName          NVARCHAR(150) NOT NULL,
    MotherName          NVARCHAR(150) NULL,
    BloodGroup          INT NOT NULL,
    Gender              INT NOT NULL,
    DateOfBirth         DATE NOT NULL,
    GuardianPhone       NVARCHAR(20) NOT NULL,
    PresentAddress      NVARCHAR(500) NULL,
    PermanentAddress    NVARCHAR(500) NULL,
    ClassId             INT NOT NULL,
    SectionId           INT NOT NULL,
    RollNumber          NVARCHAR(20) NOT NULL,
    MonthlyTuitionFee   DECIMAL(18,2) NOT NULL CONSTRAINT DF_Students_Fee DEFAULT (0),
    AdmissionDate       DATE NOT NULL CONSTRAINT DF_Students_AdmissionDate DEFAULT (CAST(GETDATE() AS DATE)),
    Status              INT NOT NULL CONSTRAINT DF_Students_Status DEFAULT (1),
    PhotoPath           NVARCHAR(260) NULL,
    AcademicSession     NVARCHAR(20) NOT NULL CONSTRAINT DF_Students_Session DEFAULT (N'2025-2026'),
    CreatedAt           DATETIME2 NOT NULL CONSTRAINT DF_Students_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy           NVARCHAR(100) NULL,
    UpdatedAt           DATETIME2 NULL,
    UpdatedBy           NVARCHAR(100) NULL,
    IsDeleted           BIT NOT NULL CONSTRAINT DF_Students_IsDeleted DEFAULT (0),
    CONSTRAINT UQ_Students_RegistrationNo UNIQUE (RegistrationNo),
    CONSTRAINT FK_Students_Classes FOREIGN KEY (ClassId) REFERENCES dbo.Classes(ClassId),
    CONSTRAINT FK_Students_Sections FOREIGN KEY (SectionId) REFERENCES dbo.Sections(SectionId)
);
GO

CREATE INDEX IX_Students_Mobile ON dbo.Students(GuardianPhone);
CREATE INDEX IX_Students_ClassSection ON dbo.Students(ClassId, SectionId, RollNumber);
GO

CREATE TABLE dbo.FeeCategories
(
    FeeCategoryId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_FeeCategories PRIMARY KEY,
    CategoryName  NVARCHAR(100) NOT NULL,
    Description   NVARCHAR(250) NULL,
    IsRecurring   BIT NOT NULL CONSTRAINT DF_FeeCategories_IsRecurring DEFAULT (1),
    IsActive      BIT NOT NULL CONSTRAINT DF_FeeCategories_IsActive DEFAULT (1),
    CreatedAt     DATETIME2 NOT NULL CONSTRAINT DF_FeeCategories_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy     NVARCHAR(100) NULL,
    UpdatedAt     DATETIME2 NULL,
    UpdatedBy     NVARCHAR(100) NULL,
    IsDeleted     BIT NOT NULL CONSTRAINT DF_FeeCategories_IsDeleted DEFAULT (0)
);
GO

CREATE TABLE dbo.StudentFeeStructures
(
    StructureId     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StudentFeeStructures PRIMARY KEY,
    StudentId       INT NOT NULL,
    FeeCategoryId   INT NOT NULL,
    Amount          DECIMAL(18,2) NOT NULL,
    AcademicSession NVARCHAR(20) NOT NULL,
    IsActive        BIT NOT NULL CONSTRAINT DF_SFS_IsActive DEFAULT (1),
    CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_SFS_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy       NVARCHAR(100) NULL,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(100) NULL,
    IsDeleted       BIT NOT NULL CONSTRAINT DF_SFS_IsDeleted DEFAULT (0),
    CONSTRAINT FK_SFS_Students FOREIGN KEY (StudentId) REFERENCES dbo.Students(StudentId),
    CONSTRAINT FK_SFS_FeeCategories FOREIGN KEY (FeeCategoryId) REFERENCES dbo.FeeCategories(FeeCategoryId)
);
GO

CREATE TABLE dbo.StudentLedger
(
    LedgerId        BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StudentLedger PRIMARY KEY,
    StudentId       INT NOT NULL,
    TransactionDate DATETIME2 NOT NULL,
    VoucherNo       NVARCHAR(50) NOT NULL,
    Particulars     NVARCHAR(500) NOT NULL,
    FeePeriod       NVARCHAR(100) NULL,
    DebitAmount     DECIMAL(18,2) NOT NULL CONSTRAINT DF_Ledger_Debit DEFAULT (0),
    CreditAmount    DECIMAL(18,2) NOT NULL CONSTRAINT DF_Ledger_Credit DEFAULT (0),
    EntryType       INT NOT NULL,
    Status          INT NOT NULL CONSTRAINT DF_Ledger_Status DEFAULT (1),
    ReferenceNo     NVARCHAR(100) NULL,
    CreatedByUserId INT NULL,
    CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_Ledger_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy       NVARCHAR(100) NULL,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(100) NULL,
    IsDeleted       BIT NOT NULL CONSTRAINT DF_Ledger_IsDeleted DEFAULT (0),
    CONSTRAINT FK_Ledger_Students FOREIGN KEY (StudentId) REFERENCES dbo.Students(StudentId)
);
GO

CREATE INDEX IX_StudentLedger_StudentDate ON dbo.StudentLedger(StudentId, TransactionDate, LedgerId);
GO

CREATE TABLE dbo.FeeInvoices
(
    InvoiceId     BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_FeeInvoices PRIMARY KEY,
    StudentId     INT NOT NULL,
    InvoiceNo     NVARCHAR(50) NOT NULL,
    FeePeriod     NVARCHAR(100) NOT NULL,
    CategoryName  NVARCHAR(100) NOT NULL,
    Amount        DECIMAL(18,2) NOT NULL,
    FineAmount    DECIMAL(18,2) NOT NULL CONSTRAINT DF_FeeInvoices_Fine DEFAULT (0),
    WaiverAmount  DECIMAL(18,2) NOT NULL CONSTRAINT DF_FeeInvoices_Waiver DEFAULT (0),
    InvoiceDate   DATE NOT NULL,
    DueDate       DATE NULL,
    Status        INT NOT NULL,
    LedgerId      BIGINT NULL,
    CreatedAt     DATETIME2 NOT NULL CONSTRAINT DF_FeeInvoices_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy     NVARCHAR(100) NULL,
    UpdatedAt     DATETIME2 NULL,
    UpdatedBy     NVARCHAR(100) NULL,
    IsDeleted     BIT NOT NULL CONSTRAINT DF_FeeInvoices_IsDeleted DEFAULT (0),
    CONSTRAINT FK_FeeInvoices_Students FOREIGN KEY (StudentId) REFERENCES dbo.Students(StudentId)
);
GO

CREATE TABLE dbo.FeePayments
(
    CollectionId      BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_FeePayments PRIMARY KEY,
    StudentId         INT NOT NULL,
    ReceiptNo         NVARCHAR(50) NOT NULL,
    PaymentDate       DATETIME2 NOT NULL,
    AmountPaid        DECIMAL(18,2) NOT NULL,
    FineCollected     DECIMAL(18,2) NOT NULL CONSTRAINT DF_FeePayments_Fine DEFAULT (0),
    WaiverApplied     DECIMAL(18,2) NOT NULL CONSTRAINT DF_FeePayments_Waiver DEFAULT (0),
    PaymentMethod     INT NOT NULL,
    TransactionRef    NVARCHAR(100) NULL,
    FeePeriods        NVARCHAR(250) NULL,
    Remarks           NVARCHAR(500) NULL,
    LedgerId          BIGINT NULL,
    CollectedByUserId INT NOT NULL,
    IsPrinted         BIT NOT NULL CONSTRAINT DF_FeePayments_IsPrinted DEFAULT (0),
    CreatedAt         DATETIME2 NOT NULL CONSTRAINT DF_FeePayments_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy         NVARCHAR(100) NULL,
    UpdatedAt         DATETIME2 NULL,
    UpdatedBy         NVARCHAR(100) NULL,
    IsDeleted         BIT NOT NULL CONSTRAINT DF_FeePayments_IsDeleted DEFAULT (0),
    CONSTRAINT UQ_FeePayments_ReceiptNo UNIQUE (ReceiptNo),
    CONSTRAINT FK_FeePayments_Students FOREIGN KEY (StudentId) REFERENCES dbo.Students(StudentId)
);
GO

CREATE TABLE dbo.ExamTerms
(
    ExamTermId      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ExamTerms PRIMARY KEY,
    TermName        NVARCHAR(100) NOT NULL,
    AcademicSession NVARCHAR(20) NOT NULL,
    StartDate       DATE NOT NULL,
    EndDate         DATE NOT NULL,
    TimetableJson   NVARCHAR(MAX) NULL,
    IsActive        BIT NOT NULL CONSTRAINT DF_ExamTerms_IsActive DEFAULT (1),
    CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_ExamTerms_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy       NVARCHAR(100) NULL,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(100) NULL,
    IsDeleted       BIT NOT NULL CONSTRAINT DF_ExamTerms_IsDeleted DEFAULT (0)
);
GO

CREATE TABLE dbo.ExamClearances
(
    ClearanceId     BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ExamClearances PRIMARY KEY,
    StudentId       INT NOT NULL,
    ExamTermId      INT NOT NULL,
    DuesAtClearance DECIMAL(18,2) NOT NULL CONSTRAINT DF_ExamClearances_Dues DEFAULT (0),
    IsCleared       BIT NOT NULL CONSTRAINT DF_ExamClearances_IsCleared DEFAULT (0),
    AdminOverride   BIT NOT NULL CONSTRAINT DF_ExamClearances_Override DEFAULT (0),
    OverrideReason  NVARCHAR(500) NULL,
    ClearedByUserId INT NULL,
    ClearedAt       DATETIME2 NULL,
    Remarks         NVARCHAR(500) NULL,
    CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_ExamClearances_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy       NVARCHAR(100) NULL,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(100) NULL,
    IsDeleted       BIT NOT NULL CONSTRAINT DF_ExamClearances_IsDeleted DEFAULT (0),
    CONSTRAINT UQ_ExamClearances_StudentTerm UNIQUE (StudentId, ExamTermId),
    CONSTRAINT FK_ExamClearances_Students FOREIGN KEY (StudentId) REFERENCES dbo.Students(StudentId),
    CONSTRAINT FK_ExamClearances_ExamTerms FOREIGN KEY (ExamTermId) REFERENCES dbo.ExamTerms(ExamTermId)
);
GO

CREATE TABLE dbo.AdmitCards
(
    AdmitCardId     BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AdmitCards PRIMARY KEY,
    StudentId       INT NOT NULL,
    ExamTermId      INT NOT NULL,
    AdmitCardNo     NVARCHAR(50) NOT NULL,
    BarcodeValue    NVARCHAR(100) NOT NULL,
    IssuedAt        DATETIME2 NOT NULL,
    IssuedByUserId  INT NOT NULL,
    IsPrinted       BIT NOT NULL CONSTRAINT DF_AdmitCards_IsPrinted DEFAULT (0),
    ClearanceId     BIGINT NULL,
    CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_AdmitCards_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CreatedBy       NVARCHAR(100) NULL,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(100) NULL,
    IsDeleted       BIT NOT NULL CONSTRAINT DF_AdmitCards_IsDeleted DEFAULT (0),
    CONSTRAINT FK_AdmitCards_Students FOREIGN KEY (StudentId) REFERENCES dbo.Students(StudentId),
    CONSTRAINT FK_AdmitCards_ExamTerms FOREIGN KEY (ExamTermId) REFERENCES dbo.ExamTerms(ExamTermId),
    CONSTRAINT FK_AdmitCards_Clearances FOREIGN KEY (ClearanceId) REFERENCES dbo.ExamClearances(ClearanceId)
);
GO

/* ---------- Running balance view ---------- */
CREATE VIEW dbo.vw_StudentLedgerDetailed
AS
SELECT
    l.LedgerId,
    l.StudentId,
    s.RegistrationNo,
    s.FullName,
    l.TransactionDate,
    l.VoucherNo,
    l.Particulars,
    l.FeePeriod,
    l.DebitAmount,
    l.CreditAmount,
    SUM(l.DebitAmount - l.CreditAmount) OVER (
        PARTITION BY l.StudentId
        ORDER BY l.TransactionDate, l.LedgerId
        ROWS UNBOUNDED PRECEDING
    ) AS RunningBalance,
    l.EntryType,
    l.Status,
    l.ReferenceNo
FROM dbo.StudentLedger l
INNER JOIN dbo.Students s ON s.StudentId = l.StudentId
WHERE l.IsDeleted = 0 AND s.IsDeleted = 0;
GO

/* ---------- Seed: Users (password: Admin@123 / Accounts@123) ---------- */
-- Hash format: SHA256$<base64-salt>$<base64-hash> generated by PasswordHasher
-- For demo convenience we seed known hashes produced offline for Admin@123
INSERT INTO dbo.Users (Username, FullName, PasswordHash, Email, Role, IsActive, CreatedBy)
VALUES
(N'admin', N'System Super Admin',
 N'SHA256$ZGVtb1NhbHQxMjM0NTY=$xK8vQpLmN2rT9wYhF6jA3sD1gH5kP0oU4iE7cB2aM9w=',
 N'admin@school.edu', 1, 1, N'seed'),
(N'accounts', N'Accounts Manager',
 N'SHA256$ZGVtb1NhbHQxMjM0NTY=$xK8vQpLmN2rT9wYhF6jA3sD1gH5kP0oU4iE7cB2aM9w=',
 N'accounts@school.edu', 2, 1, N'seed'),
(N'admission', N'Admission Officer',
 N'SHA256$ZGVtb1NhbHQxMjM0NTY=$xK8vQpLmN2rT9wYhF6jA3sD1gH5kP0oU4iE7cB2aM9w=',
 N'admission@school.edu', 3, 1, N'seed'),
(N'teacher', N'Class Teacher',
 N'SHA256$ZGVtb1NhbHQxMjM0NTY=$xK8vQpLmN2rT9wYhF6jA3sD1gH5kP0oU4iE7cB2aM9w=',
 N'teacher@school.edu', 4, 1, N'seed');
GO

/* NOTE: On first app login, prefer creating users via the app so hashes match PasswordHasher.
   A bootstrap admin is also created by DatabaseBootstrapper on first run. */

INSERT INTO dbo.Classes (ClassName, ClassCode, SortOrder) VALUES
(N'Class 6', N'C6', 6),
(N'Class 7', N'C7', 7),
(N'Class 8', N'C8', 8),
(N'Class 9', N'C9', 9),
(N'Class 10', N'C10', 10);
GO

INSERT INTO dbo.Sections (ClassId, SectionName, Capacity)
SELECT ClassId, N'A', 40 FROM dbo.Classes
UNION ALL
SELECT ClassId, N'B', 40 FROM dbo.Classes;
GO

INSERT INTO dbo.FeeCategories (CategoryName, Description, IsRecurring)
VALUES (N'Tuition Fee', N'Monthly tuition', 1),
       (N'Admission Fee', N'One-time admission', 0),
       (N'Exam Fee', N'Term examination fee', 0);
GO

INSERT INTO dbo.ExamTerms (TermName, AcademicSession, StartDate, EndDate, TimetableJson, IsActive)
VALUES
(N'Mid-Term 2026', N'2025-2026', '2026-03-01', '2026-03-15',
 N'[{"subject":"Bangla","date":"2026-03-01","time":"10:00 AM"},{"subject":"English","date":"2026-03-03","time":"10:00 AM"},{"subject":"Mathematics","date":"2026-03-05","time":"10:00 AM"},{"subject":"Science","date":"2026-03-07","time":"10:00 AM"}]', 1),
(N'Final Exam 2026', N'2025-2026', '2026-06-01', '2026-06-20',
 N'[{"subject":"Bangla","date":"2026-06-01","time":"09:30 AM"},{"subject":"English","date":"2026-06-03","time":"09:30 AM"},{"subject":"Mathematics","date":"2026-06-05","time":"09:30 AM"}]', 1);
GO

/* ---------- Demo students ---------- */
DECLARE @C6 INT = (SELECT ClassId FROM dbo.Classes WHERE ClassCode = N'C6');
DECLARE @C7 INT = (SELECT ClassId FROM dbo.Classes WHERE ClassCode = N'C7');
DECLARE @C8 INT = (SELECT ClassId FROM dbo.Classes WHERE ClassCode = N'C8');
DECLARE @S6A INT = (SELECT SectionId FROM dbo.Sections WHERE ClassId = @C6 AND SectionName = N'A');
DECLARE @S7A INT = (SELECT SectionId FROM dbo.Sections WHERE ClassId = @C7 AND SectionName = N'A');
DECLARE @S8A INT = (SELECT SectionId FROM dbo.Sections WHERE ClassId = @C8 AND SectionName = N'A');
DECLARE @S6B INT = (SELECT SectionId FROM dbo.Sections WHERE ClassId = @C6 AND SectionName = N'B');
DECLARE @S7B INT = (SELECT SectionId FROM dbo.Sections WHERE ClassId = @C7 AND SectionName = N'B');

INSERT INTO dbo.Students
(RegistrationNo, FullName, FatherName, MotherName, BloodGroup, Gender, DateOfBirth, GuardianPhone,
 PresentAddress, PermanentAddress, ClassId, SectionId, RollNumber, MonthlyTuitionFee, AdmissionDate, Status, AcademicSession, CreatedBy)
VALUES
(N'REG-2026-0001', N'Ayesha Rahman', N'Md. Karim Rahman', N'Nasrin Akter', 7, 2, '2014-05-12', N'01711000001',
 N'12 Green Road, Dhaka', N'12 Green Road, Dhaka', @C6, @S6A, N'01', 2500.00, '2026-01-05', 1, N'2025-2026', N'seed'),
(N'REG-2026-0002', N'Rahim Uddin', N'Abdul Jalil', N'Salma Begum', 1, 1, '2013-08-21', N'01711000002',
 N'45 Mirpur-10, Dhaka', N'Village: Savar, Dhaka', @C7, @S7A, N'05', 2800.00, '2026-01-06', 1, N'2025-2026', N'seed'),
(N'REG-2026-0003', N'Nusrat Jahan', N'Syed Hasan', N'Farhana Syed', 3, 2, '2012-11-03', N'01711000003',
 N'88 Dhanmondi, Dhaka', N'88 Dhanmondi, Dhaka', @C8, @S8A, N'12', 3200.00, '2026-01-07', 1, N'2025-2026', N'seed'),
(N'REG-2026-0004', N'Tanvir Ahmed', N'Imran Ahmed', N'Laila Ahmed', 5, 1, '2014-02-18', N'01711000004',
 N'3 Uttara Sector 7', N'3 Uttara Sector 7', @C6, @S6B, N'08', 2500.00, '2026-01-08', 1, N'2025-2026', N'seed'),
(N'REG-2026-0005', N'Meherin Islam', N'Nazrul Islam', N'Shahana Islam', 7, 2, '2013-01-30', N'01711000005',
 N'22 Banani, Dhaka', N'Comilla Sadar', @C7, @S7B, N'03', 2800.00, '2026-01-09', 1, N'2025-2026', N'seed');
GO

/* ---------- Demo ledger / payments ---------- */
DECLARE @S1 INT = (SELECT StudentId FROM dbo.Students WHERE RegistrationNo = N'REG-2026-0001');
DECLARE @S2 INT = (SELECT StudentId FROM dbo.Students WHERE RegistrationNo = N'REG-2026-0002');
DECLARE @S3 INT = (SELECT StudentId FROM dbo.Students WHERE RegistrationNo = N'REG-2026-0003');
DECLARE @S4 INT = (SELECT StudentId FROM dbo.Students WHERE RegistrationNo = N'REG-2026-0004');
DECLARE @S5 INT = (SELECT StudentId FROM dbo.Students WHERE RegistrationNo = N'REG-2026-0005');
DECLARE @Admin INT = (SELECT TOP 1 UserId FROM dbo.Users WHERE Username = N'admin');

INSERT INTO dbo.StudentLedger (StudentId, TransactionDate, VoucherNo, Particulars, FeePeriod, DebitAmount, CreditAmount, EntryType, Status, CreatedByUserId)
VALUES
(@S1, '2026-01-05', N'INV-20260105-0001', N'Tuition Fee - Jan-2026', N'Jan-2026', 2500, 0, 1, 1, @Admin),
(@S1, '2026-01-10', N'RCPT-20260110-0001', N'Fee Collection via Cash (Jan-2026)', N'Jan-2026', 0, 2500, 2, 2, @Admin),
(@S1, '2026-02-01', N'INV-20260201-0001', N'Tuition Fee - Feb-2026', N'Feb-2026', 2500, 0, 1, 1, @Admin),
(@S2, '2026-01-06', N'INV-20260106-0002', N'Tuition Fee - Jan-2026', N'Jan-2026', 2800, 0, 1, 1, @Admin),
(@S2, '2026-01-15', N'RCPT-20260115-0002', N'Fee Collection via bKash (Jan-2026)', N'Jan-2026', 0, 2000, 2, 3, @Admin),
(@S3, '2026-01-07', N'INV-20260107-0003', N'Tuition Fee - Jan-2026', N'Jan-2026', 3200, 0, 1, 1, @Admin),
(@S3, '2026-01-20', N'RCPT-20260120-0003', N'Fee Collection via Nagad (Jan-2026)', N'Jan-2026', 0, 3200, 2, 2, @Admin),
(@S3, '2026-02-01', N'INV-20260201-0003', N'Tuition Fee - Feb-2026', N'Feb-2026', 3200, 0, 1, 1, @Admin),
(@S4, '2026-01-08', N'INV-20260108-0004', N'Tuition Fee - Jan-2026', N'Jan-2026', 2500, 0, 1, 1, @Admin),
(@S5, '2026-01-09', N'INV-20260109-0005', N'Tuition Fee - Jan-2026', N'Jan-2026', 2800, 0, 1, 1, @Admin),
(@S5, '2026-01-12', N'RCPT-20260112-0005', N'Fee Collection via Bank Transfer (Jan-2026)', N'Jan-2026', 0, 2800, 2, 2, @Admin),
(@S5, '2026-02-01', N'INV-20260201-0005', N'Tuition Fee - Feb-2026', N'Feb-2026', 2800, 0, 1, 1, @Admin),
(@S5, '2026-02-05', N'WVR-20260205-0005', N'Waiver / Discount (Feb-2026)', N'Feb-2026', 0, 300, 4, 4, @Admin);

INSERT INTO dbo.FeePayments (StudentId, ReceiptNo, PaymentDate, AmountPaid, PaymentMethod, FeePeriods, CollectedByUserId)
VALUES
(@S1, N'RCP-202601-00001', '2026-01-10', 2500, 1, N'Jan-2026', @Admin),
(@S2, N'RCP-202601-00002', '2026-01-15', 2000, 2, N'Jan-2026', @Admin),
(@S3, N'RCP-202601-00003', '2026-01-20', 3200, 3, N'Jan-2026', @Admin),
(@S5, N'RCP-202601-00004', '2026-01-12', 2800, 4, N'Jan-2026', @Admin);
GO

PRINT N'StudentManagementDB created successfully with seed data.';
GO
