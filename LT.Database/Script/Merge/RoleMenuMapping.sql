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