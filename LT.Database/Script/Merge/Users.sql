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