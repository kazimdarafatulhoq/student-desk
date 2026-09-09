/*
  Run once in SSMS as SQL admin (sa) or Windows Administrator.
  Creates the database your App.config points to and grants login [anik] access.
*/

IF DB_ID(N'StudentManagementSDB') IS NULL
BEGIN
    CREATE DATABASE StudentManagementSDB;
END
GO

USE StudentManagementSDB;
GO

-- Grant SQL login [anik] db_owner on this database (ignore if login missing)
IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = N'anik')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'anik')
        CREATE USER [anik] FOR LOGIN [anik];

    IF IS_ROLEMEMBER(N'db_owner', N'anik') = 0 OR IS_ROLEMEMBER(N'db_owner', N'anik') IS NULL
        ALTER ROLE db_owner ADD MEMBER [anik];
END
GO

PRINT N'StudentManagementSDB is ready for user [anik]. Restart the desktop app (or run StudentManagementDB.sql for full seed).';
GO
