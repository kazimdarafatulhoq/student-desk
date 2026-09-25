# Student Management & Financial Accounting

Enterprise desktop application for school admissions, fee POS collection, student ledgers, exam clearance, and admit-card printing.

Built with **C# / .NET 5 Windows Forms**, **EF Core 5 + SQL Server 2022**, and a clean multi-tier architecture.

## Solution structure

| Project | Responsibility |
|---------|----------------|
| `StudentManagement.Domain` | Entities & enums |
| `StudentManagement.Application` | DTOs, interfaces, business services, password hashing |
| `StudentManagement.Infrastructure` | EF Core `DbContext`, repositories, unit of work, bootstrapper |
| `StudentManagement.Reporting` | QuestPDF receipts, ledger statements, admit cards; CSV export |
| `StudentManagement.Desktop` | WinForms UI (Slate-900 dark theme), MDI-style shell |

## Prerequisites

- Windows 10/11
- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) (5.0.408 or later 5.0.x)
- SQL Server 2022 (LocalDB, Express, or full)
- Visual Studio 2019/2022 with .NET desktop development workload, or `dotnet` CLI

## Database setup

Your Windows login may not have `CREATE DATABASE` permission. Create the database once as an admin:

1. Open **SQL Server Management Studio** as Administrator (or connect with `sa`).
2. Run [`database/CreateDatabaseAndGrantAccess.sql`](database/CreateDatabaseAndGrantAccess.sql) — creates `StudentManagementSDB` and grants login `[anik]`.
3. Run [`database/StudentManagementSDB.sql`](database/StudentManagementSDB.sql) — tables, views, stored procedures, and demo seed.
4. Confirm the connection string in:
   - `src/StudentManagement.Desktop/appsettings.json`
   - `src/StudentManagement.Desktop/App.config`
5. Set `OfflineMode` to `false` when SQL Server is available.

See [`database/README.md`](database/README.md) for object list and SP reference.

Configured connection (matches this machine):

```
Data Source=NGBC-IT-168\MSSQLSERVER1;Initial Catalog=StudentManagementSDB;User Id=anik;Password=***;TrustServerCertificate=True;MultipleActiveResultSets=True;
```

Local alternatives:

```
Server=localhost\SQLEXPRESS;Database=StudentManagementSDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

```
Server=(localdb)\MSSQLLocalDB;Database=StudentManagementSDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

On startup the app re-hashes demo users and fills any missing lookup rows when the database is reachable.

### Default login

| Username | Password | Role |
|----------|----------|------|
| `admin` | `Admin@123` | Super Admin |
| `accounts` | `Accounts@123` | Accounts Manager |
| `admission` | `Admission@123` | Admission Officer |
| `teacher` | `Teacher@123` | Teacher |

> SQL seed password hashes are placeholders. `DatabaseBootstrapper` replaces them on first app start so the passwords above work.

## Run

```bash
dotnet restore StudentManagement.sln
dotnet build StudentManagement.sln -c Release
dotnet run --project src/StudentManagement.Desktop -c Release
```

Or open `StudentManagement.sln` in Visual Studio and set **StudentManagement.Desktop** as the startup project.

## Modules

- **Dashboard** — active students & receivables snapshot  
- **Student Admission** — guardian info, address copy, class/section, auto `REG-YYYY-XXXX`  
- **Student Search** — filter/search grid with ledger / fee / admit actions  
- **Fee Collection** — month matrix, fine/waiver, Cash/bKash/Nagad/Bank, PDF receipt  
- **Student Ledger** — debit/credit running balance, statement PDF, CSV export  
- **Exam Clearance** — blocks admit cards when dues &gt; ৳0 unless admin override  
- **Admit Card Print** — single/batch PDF with timetable + barcode value  
- **User Management** — RBAC (Super Admin, Accounts Manager, Admission Officer, Teacher), SHA-256 salted passwords  

## UI theme

`UITheme.cs` applies a Tailwind Slate-900 palette:

- Canvas `#0F172A` · Card `#1E293B` · Border `#334155`  
- Primary `#2563EB` / Indigo `#4F46E5`  
- Success `#10B981` · Danger `#F43F5E`  
- Segoe UI typography; styled `DataGridView` headers, alternating rows, selection  

## Reports output

PDF/CSV files are written to:

`%USERPROFILE%\Documents\StudentManagement\Reports`


## WinForms designer (Visual Studio)

All `*.Designer.cs` files use classic Visual Studio–compatible `InitializeComponent` code (no helper methods, no object initializers). Theme styling (`UITheme.ApplyForm`) runs only at runtime, not at design time.

If a form designer still fails to load after pull:

1. Close all designer tabs
2. **Build → Clean Solution**, then **Rebuild Solution**
3. Open the form again (right-click → View Designer)

Requires the **.NET desktop development** workload. For .NET 5 WinForms, Visual Studio 2019 16.8+ or Visual Studio 2022 is recommended.

## Architecture notes

- Repositories + `IUnitOfWork` isolate SQL Server access  
- Fee collection posts invoice / waiver / payment ledger rows in one transaction  
- `vw_StudentLedgerDetailed` / `vw_StudentBalanceSummary` provide SQL running balances  
- Stored procedures cover registration/receipt/invoice numbering, ledger, dues, dashboard, and fee posting (`database/StudentManagementSDB.sql`)  
- Desktop hosts child forms in `pnlContentContainer` (no MDI title-bar flicker)  
