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
:r .\Merge\Roles.sql
GO
:r .\Merge\Users.sql
GO
:r .\Merge\Menu.sql
GO
:r .\Merge\RoleMenuMapping.sql
GO
