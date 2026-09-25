/*
================================================================================
 CreateDatabaseAndGrantAccess.sql
 Run once in SSMS as SQL admin (sa) or Windows Administrator.

 Creates StudentManagementSDB (matches App.config / appsettings.json) and
 grants SQL login [anik] db_owner access.

 After this script, run StudentManagementSDB.sql for full schema + seed.
================================================================================
*/

SET NOCOUNT ON;
GO

IF DB_ID(N'StudentManagementSDB') IS NULL
BEGIN
    CREATE DATABASE StudentManagementSDB;
    PRINT N'Created database StudentManagementSDB.';
END
ELSE
BEGIN
    PRINT N'Database StudentManagementSDB already exists.';
END
GO

USE StudentManagementSDB;
GO

-- Grant SQL login [anik] db_owner (no-op if login does not exist on this server)
IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = N'anik')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'anik')
        CREATE USER [anik] FOR LOGIN [anik];

    IF IS_ROLEMEMBER(N'db_owner', N'anik') = 0 OR IS_ROLEMEMBER(N'db_owner', N'anik') IS NULL
        ALTER ROLE db_owner ADD MEMBER [anik];

    PRINT N'User [anik] is db_owner on StudentManagementSDB.';
END
ELSE
BEGIN
    PRINT N'Login [anik] not found on this server — skipped user grant.';
    PRINT N'Create the login or use Windows auth, then re-run this script if needed.';
END
GO

PRINT N'';
PRINT N'Next step: execute database\StudentManagementSDB.sql for tables, views, SPs, and seed.';
PRINT N'Then set OfflineMode=false in App.config / appsettings.json and start the app.';
GO
