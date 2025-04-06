CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[roleId] [int] NULL,
	[firstName] [varchar](100) NULL,
	[lastName] [varchar](100) NULL,
	[email] [varchar](50) NULL,
	[password] [varchar](100) NULL,
	[managerid] [int],
	[profilepicture] [varchar](100) NULL,
	[facebookurl] [varchar](100) NULL,
	[twitterurl] [varchar](100) NULL,
	[linkedinurl] [varchar](100) NULL,
	[status] [int] NULL,
	[createdBy] [int] NULL,
	[createdDate] [datetime] NULL,
	[updatedBy] [int] NULL,
	[updatedDate] [datetime] NULL,
	[lastlogin] [datetime] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
