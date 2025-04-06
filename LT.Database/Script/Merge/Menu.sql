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
