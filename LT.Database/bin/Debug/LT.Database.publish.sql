﻿/*
Deployment script for legal_tech

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "legal_tech"
:setvar DefaultFilePrefix "legal_tech"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE,
                DISABLE_BROKER 
            WITH ROLLBACK IMMEDIATE;
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE (QUERY_CAPTURE_MODE = ALL, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 367), MAX_STORAGE_SIZE_MB = 100) 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE = OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
GO

GO
PRINT N'Creating Table [dbo].[Menu]...';


GO
CREATE TABLE [dbo].[Menu] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [name]           NVARCHAR (100) NULL,
    [icon]           VARCHAR (50)   NULL,
    [parentid]       INT            NULL,
    [controller]     NVARCHAR (100) NULL,
    [action]         NVARCHAR (100) NULL,
    [internalstatus] INT            NULL,
    [order]          INT            NULL,
    [status]         INT            NULL,
    [createdby]      INT            NULL,
    [createddate]    DATETIME       NULL,
    [updatedby]      INT            NULL,
    [updateddate]    DATETIME       NULL,
    CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY];


GO
PRINT N'Creating Table [dbo].[RoleMenuMapping]...';


GO
CREATE TABLE [dbo].[RoleMenuMapping] (
    [id]          INT      IDENTITY (1, 1) NOT NULL,
    [roleid]      INT      NULL,
    [menuid]      INT      NULL,
    [status]      INT      NULL,
    [createdby]   INT      NULL,
    [createddate] DATETIME NULL,
    [updatedby]   INT      NULL,
    [updateddate] DATETIME NULL,
    CONSTRAINT [PK_RoleMenuMapping] PRIMARY KEY CLUSTERED ([id] ASC) ON [PRIMARY]
) ON [PRIMARY];


GO
PRINT N'Creating Table [dbo].[Roles]...';


GO
CREATE TABLE [dbo].[Roles] (
    [id]                INT           IDENTITY (1, 1) NOT NULL,
    [name]              VARCHAR (100) NULL,
    [status]            INT           NULL,
    [newstatus]         INT           NULL,
    [modifystatus]      INT           NULL,
    [deletestatus]      INT           NULL,
    [impersonatestatus] INT           NULL,
    [viewstatus]        INT           NULL,
    [createdby]         INT           NULL,
    [createddate]       DATETIME      NULL,
    [updatedby]         INT           NULL,
    [updateddate]       DATETIME      NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([id] ASC) ON [PRIMARY]
) ON [PRIMARY];


GO
PRINT N'Creating Table [dbo].[UserCourtCaseDetails]...';


GO
CREATE TABLE [dbo].[UserCourtCaseDetails] (
    [CaseID]             INT            IDENTITY (1, 1) NOT NULL,
    [UserID]             INT            NOT NULL,
    [CaseTitle]          NVARCHAR (255) NOT NULL,
    [CaseType]           NVARCHAR (100) NOT NULL,
    [CourtName]          NVARCHAR (255) NOT NULL,
    [FilingDate]         DATETIME       NOT NULL,
    [HearingDate]        DATETIME       NULL,
    [CaseStatus]         NVARCHAR (50)  NOT NULL,
    [LawyerName]         NVARCHAR (255) NULL,
    [OpponentName]       NVARCHAR (255) NULL,
    [OpponentLawyerName] NVARCHAR (255) NULL,
    [CaseDescription]    NVARCHAR (MAX) NULL,
    [VerdictDate]        DATETIME       NULL,
    [VerdictDetails]     NVARCHAR (MAX) NULL,
    [CreatedDate]        DATETIME       NOT NULL,
    [ModifiedDate]       DATETIME       NOT NULL,
    [IsActive]           BIT            NOT NULL,
    [Remarks]            NVARCHAR (MAX) NULL,
    [pdf]                VARCHAR (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([CaseID] ASC) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];


GO
PRINT N'Creating Table [dbo].[Users]...';


GO
CREATE TABLE [dbo].[Users] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [roleId]         INT           NULL,
    [firstName]      VARCHAR (100) NULL,
    [lastName]       VARCHAR (100) NULL,
    [email]          VARCHAR (50)  NULL,
    [password]       VARCHAR (100) NULL,
    [managerid]      INT           NULL,
    [profilepicture] VARCHAR (100) NULL,
    [facebookurl]    VARCHAR (100) NULL,
    [twitterurl]     VARCHAR (100) NULL,
    [linkedinurl]    VARCHAR (100) NULL,
    [status]         INT           NULL,
    [createdBy]      INT           NULL,
    [createdDate]    DATETIME      NULL,
    [updatedBy]      INT           NULL,
    [updatedDate]    DATETIME      NULL,
    [lastlogin]      DATETIME      NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([id] ASC) ON [PRIMARY]
) ON [PRIMARY];


GO
PRINT N'Creating Table [dbo].[UserSession]...';


GO
CREATE TABLE [dbo].[UserSession] (
    [id]        INT           IDENTITY (1, 1) NOT NULL,
    [userid]    VARCHAR (100) NOT NULL,
    [sessionid] VARCHAR (100) NULL,
    CONSTRAINT [PK_UserSession] PRIMARY KEY CLUSTERED ([id] ASC) ON [PRIMARY]
) ON [PRIMARY];


GO
PRINT N'Creating Default Constraint <unnamed>...';


GO
ALTER TABLE [dbo].[UserCourtCaseDetails]
    ADD DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Creating Default Constraint <unnamed>...';


GO
ALTER TABLE [dbo].[UserCourtCaseDetails]
    ADD DEFAULT ((1)) FOR [IsActive];


GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
PRINT 'Post-Deployment Scripts started ...'
/* Keep Users first so system user is ready for seeding. */
SET IDENTITY_INSERT [dbo].[Roles] ON
MERGE INTO [dbo].[Roles] AS [Target]
USING (VALUES
(-1,'Admin',1,1,1,1,1,1,1,getdate(),NULL,NULL),
(-2,'Super Admin',1,1,1,1,1,1,1,getdate(),NULL,NULL),
(-3,'User',1,1,1,1,1,1,1,getdate(),NULL,NULL),
(-4,'Advocate',1,1,1,1,1,1,1,getdate(),NULL,NULL)
)AS [Source] ([id],[name],[status],newstatus,modifystatus,deletestatus,impersonatestatus,viewstatus,[createdby],[createddate],[updatedby],[updateddate])
ON ([Target].[Id] = [Source].[Id])
WHEN MATCHED 
THEN UPDATE SET 
[name] =Source.[name],
[status] =Source.[status],
[createdby] =Source.[createdby],
[createddate] = Source.[createddate],
[updatedby] = Source.[updatedby],
[updateddate] = Source.[updateddate],
newstatus = Source.newstatus,
modifystatus = Source.modifystatus,
deletestatus = Source.deletestatus,
impersonatestatus = Source.impersonatestatus,
viewstatus = Source.viewstatus
WHEN NOT MATCHED BY TARGET THEN
INSERT([id],[name],[status],[createdby],[createddate],[updatedby],[updateddate],newstatus,modifystatus,deletestatus,impersonatestatus,viewstatus)
 VALUES([id],[name],[status],[createdby],[createddate],[updatedby],[updateddate],newstatus,modifystatus,deletestatus,impersonatestatus,viewstatus);
SET IDENTITY_INSERT [dbo].[Roles] OFF

GO
SET IDENTITY_INSERT [dbo].[Users] ON
MERGE INTO [dbo].[Users] AS [Target]
USING (VALUES
(1,-2,'JeetYadu','Admin','mrjeet@yadu.in','827ccb0eea8a706c4c34a16891f84e7b',null,null,1,-1,null,null,null,null)
--Password is 12345
)AS [Source] ([id],[roleId],[firstName],[lastName],[email],[password],[profilepicture],[managerid],[status],[createdBy],[createdDate],[updatedBy],[updatedDate],[lastlogin])
ON ([Target].[Id] = [Source].[Id])
WHEN MATCHED 
THEN UPDATE SET 
[roleId] =  Source.[roleId],
[firstName] =  Source.[firstName],
[lastName] =  Source.[lastName],
[email] =  Source.[email],
[managerid] = Source.[managerid],
[Password] =  Source.[Password],
[status] =  Source.[status],
[createdBy] =  Source.[createdBy],
[createdDate] =  Source.[createdDate],
[updatedBy] =  Source.[updatedBy],
[updatedDate] =  Source.[updatedDate]
WHEN NOT MATCHED BY TARGET THEN
INSERT([id],[roleId],[firstName],[lastName],[email],[password],[profilepicture],[managerid],[status],[createdBy],[createdDate],[updatedBy],[updatedDate],[lastlogin])
 VALUES([id],[roleId],[firstName],[lastName],[email],[password],[profilepicture],[managerid],[status],[createdBy],[createdDate],[updatedBy],[updatedDate],[lastlogin]);
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[Menu] ON
MERGE INTO [dbo].[Menu] AS [Target]
USING (VALUES
--Super Admin Menu List role id = -2
 (1,'Home','icon-home',NULL,'Dashboard','Home','0','1','1','1',NULL,NULL,NULL),

 --User Menu List role id  = -3
 (2,'Home','icon-home',NULL,'UserDashboard','Home','0','1','1','1',NULL,NULL,NULL),
 (3,'Cases','icon-doc',NULL,'UserCase','AddCase','0','1','1','1',NULL,NULL,NULL),


 --Advocate Menu List role id  = -4
 (4,'Home','icon-home',NULL,'AdvocateDashboard','Home','0','1','1','1',NULL,NULL,NULL)

)AS [Source] ([id],[name],[icon],[parentid],[controller],[action],[internalstatus],[order],[status],[createdby],[createddate],[updatedby],[updateddate])
ON ([Target].[Id] = [Source].[Id])
WHEN MATCHED 
THEN UPDATE SET 
[name]=Source.[name],
[icon]=Source.[icon],
[parentid]=Source.[parentid],
[controller]=Source.[controller],
[action]=Source.[action],
[internalstatus]=Source.[internalstatus],
[order]=Source.[order],
[status]=Source.[status],
[createdby]=Source.[createdby],
[createddate]=Source.[createddate],
[updatedby]=Source.[updatedby],
[updateddate]=Source.[updateddate]
WHEN NOT MATCHED BY TARGET THEN
INSERT([id],[name],[icon],[parentid],[controller],[action],[internalstatus],[order],[status],[createdby],[createddate],[updatedby],[updateddate])
 VALUES([id],[name],[icon],	[parentid],[controller],[action],[internalstatus],[order],[status],[createdby],[createddate],[updatedby],[updateddate]);
SET IDENTITY_INSERT [dbo].[Menu] OFF

GO
SET IDENTITY_INSERT [dbo].[RoleMenuMapping] ON
MERGE INTO [dbo].[RoleMenuMapping] AS [Target]
USING (VALUES
 (1,'-1','1','1',NULL,NULL,NULL,NULL)
,(2,'-2','1','1',NULL,NULL,NULL,NULL)
,(3,'-3','2','1',NULL,NULL,NULL,NULL)
,(4,'-3','3','1',NULL,NULL,NULL,NULL)
,(5,'-4','4','1',NULL,NULL,NULL,NULL)


)AS [Source] ([id],[roleid],[menuid],[status],[createdby],[createddate],[updatedby],[updateddate])
ON ([Target].[Id] = [Source].[Id])
WHEN MATCHED 
THEN UPDATE SET 
[roleid] = Source.[roleid],
[menuid] = Source.[menuid],
[status] = Source.[status],
[createdby] = Source.[createdby],
[createddate] = Source.[createddate],
[updatedby] = Source.[updatedby],
[updateddate] = Source.[updateddate]
WHEN NOT MATCHED BY TARGET THEN
INSERT([id],[roleid],[menuid],[status],[createdby],[createddate],[updatedby],[updateddate])
 VALUES([id],[roleid],[menuid],[status],[createdby],[createddate],[updatedby],[updateddate]);
SET IDENTITY_INSERT [dbo].[RoleMenuMapping] OFF
GO

GO
PRINT N'Update complete.';


GO
