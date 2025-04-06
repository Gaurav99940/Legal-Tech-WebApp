CREATE TABLE [dbo].[Constitution](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ConstitutionNumber] [nvarchar](max) NOT NULL,
	[ConstitutionName] [nvarchar](max) NOT NULL,
	[ConstitutionDescription] [nvarchar](max) NOT NULL,
	[ConstitutionDetails] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Constitution] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Constitution] ADD  DEFAULT (N'') FOR [ConstitutionDetails]
GO