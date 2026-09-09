/*
  Run this once in SSMS while connected as an administrator (Windows Admin or 'sa').
  Creates StudentManagementDB and grants your Windows login db_owner access.
*/

IF DB_ID(N'StudentManagementDB') IS NULL
BEGIN
    CREATE DATABASE StudentManagementDB;
END
GO

USE StudentManagementDB;
GO

-- Grant the current Windows login access (skip if using SQL auth / sa)
DECLARE @login sysname = SUSER_SNAME();
IF @login IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @login)
BEGIN
    DECLARE @sql nvarchar(500) =
        N'CREATE USER ' + QUOTENAME(@login) + N' FOR LOGIN ' + QUOTENAME(@login) + N';' +
        N'ALTER ROLE db_owner ADD MEMBER ' + QUOTENAME(@login) + N';';
    EXEC sys.sp_executesql @sql;
END
GO

PRINT N'StudentManagementDB is ready. Now run StudentManagementDB.sql for full schema + seed data (or restart the app to let EF create tables).';
GO
