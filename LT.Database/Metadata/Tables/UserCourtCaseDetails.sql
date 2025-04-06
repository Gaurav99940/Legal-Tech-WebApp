CREATE TABLE [dbo].[UserCourtCaseDetails](
	[CaseID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[CaseTitle] [nvarchar](255) NOT NULL,
	[CaseType] [nvarchar](100) NOT NULL,
	[CourtName] [nvarchar](255) NOT NULL,
	[FilingDate] [datetime] NOT NULL,
	[HearingDate] [datetime] NULL,
	[CaseStatus] [nvarchar](50) NOT NULL,
	[LawyerName] [nvarchar](255) NULL,
	[OpponentName] [nvarchar](255) NULL,
	[OpponentLawyerName] [nvarchar](255) NULL,
	[CaseDescription] [nvarchar](max) NULL,
	[VerdictDate] [datetime] NULL,
	[VerdictDetails] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[pdf] [varchar](max) NULL
PRIMARY KEY CLUSTERED 
(
	[CaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserCourtCaseDetails] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

--ALTER TABLE [dbo].[UserCourtCaseDetails] ADD  DEFAULT (getdate()) FOR [ModifiedDate]
--GO

ALTER TABLE [dbo].[UserCourtCaseDetails] ADD  DEFAULT ((1)) FOR [IsActive]
GO
