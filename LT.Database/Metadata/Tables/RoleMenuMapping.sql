CREATE TABLE [dbo].[RoleMenuMapping](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[roleid] [int] NULL,
	[menuid] [int] NULL,
	[status] [int] NULL,
	[createdby] [int] NULL,
	[createddate] [datetime] NULL,
	[updatedby] [int] NULL,
	[updateddate] [datetime] NULL,
 CONSTRAINT [PK_RoleMenuMapping] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]