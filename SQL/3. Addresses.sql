USE [Addressbook2.0]
GO

/****** Object:  Table [dbo].[Addresses]    Script Date: 10/24/2019 12:25:05 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Addresses](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ContactID] [int] NOT NULL,
	[StreetName] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](20) NULL,
	[ZipCode] [varchar](12) NULL,
	[Type] [varchar](20) NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_Address_Contact] FOREIGN KEY([ContactID])
REFERENCES [dbo].[Contacts] ([ID])
GO

ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_Address_Contact]
GO

