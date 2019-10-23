USE [Addressbook2.0]
GO

/****** Object:  Table [dbo].[Addresses]    Script Date: 10/23/2019 10:00:25 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Addresses](
	[ID] [int] NOT NULL,
	[ContactID] [int] NOT NULL,
	[StreetName] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](12) NULL,
	[ZipCode] [varchar](10) NULL,
	[Type] [varchar](20) NULL,
 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_Addresses_Contacts] FOREIGN KEY([ContactID])
REFERENCES [dbo].[Contacts] ([ID])
GO

ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_Addresses_Contacts]
GO

