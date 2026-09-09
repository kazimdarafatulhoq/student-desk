# Student Management & Financial Accounting

Enterprise desktop application for school admissions, fee POS collection, student ledgers, exam clearance, and admit-card printing.

Built with **C# / .NET 8 Windows Forms**, **EF Core + SQL Server 2022**, and a clean multi-tier architecture.

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
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server 2022 (LocalDB, Express, or full)
- Visual Studio 2022 (recommended) or `dotnet` CLI

## Database setup

1. Open SQL Server Management Studio (or `sqlcmd`).
2. Run [`database/StudentManagementDB.sql`](database/StudentManagementDB.sql).
3. Update the connection string in:
   - `src/StudentManagement.Desktop/appsettings.json`
   - `src/StudentManagement.Desktop/App.config`

Default connection string:

```
Server=localhost\SQLEXPRESS;Database=StudentManagementDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

On first launch the app also runs `DatabaseBootstrapper`, which ensures schema/seed users exist if the database is empty.

### Default login

| Username | Password | Role |
|----------|----------|------|
| `admin` | `Admin@123` | Super Admin |
| `accounts` | `Accounts@123` | Accounts Manager |

> Prefer the bootstrapper-created hashes (or create users in **User Management**). SQL seed user hashes are placeholders; if SQL seed users fail login, use bootstrap admin or recreate via the app.

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

## Architecture notes

- Repositories + `IUnitOfWork` isolate SQL Server access  
- Fee collection posts invoice / waiver / payment ledger rows in one transaction  
- `vw_StudentLedgerDetailed` provides SQL windowed running balances  
- Desktop hosts child forms in `pnlContentContainer` (no MDI title-bar flicker)  
