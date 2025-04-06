CREATE TABLE [dbo].[Menu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[icon] [varchar](50) NULL,
	[parentid] [int] NULL,
	[controller] [nvarchar](100) NULL,
	[action] [nvarchar](100) NULL,
	[internalstatus] [int] NULL,
	[order] [int] NULL,
	[status] [int] NULL,
	[createdby] [int] NULL,
	[createddate] [datetime] NULL,
	[updatedby] [int] NULL,
	[updateddate] [datetime] NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]