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
