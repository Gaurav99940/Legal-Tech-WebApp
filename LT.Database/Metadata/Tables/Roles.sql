CREATE TABLE [dbo].[Roles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[status] [int] NULL,
	[newstatus] [int] NULL,
	[modifystatus] [int] NULL,
	[deletestatus] [int] NULL,
	[impersonatestatus] [int] NULL,
	[viewstatus] int NULL,
	[createdby] [int] NULL,
	[createddate] [datetime] NULL,
	[updatedby] [int] NULL,
	[updateddate] [datetime] NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]