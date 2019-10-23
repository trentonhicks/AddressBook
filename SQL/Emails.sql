USE [Addressbook2.0]
GO

/****** Object:  Table [dbo].[Emails]    Script Date: 10/23/2019 10:00:58 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Emails](
	[ID] [int] NOT NULL,
	[ContactID] [int] NOT NULL,
	[Text] [varchar](max) NULL,
	[Type] [varchar](20) NULL,
 CONSTRAINT [PK_Emails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Emails]  WITH CHECK ADD  CONSTRAINT [FK_Emails_Contacts] FOREIGN KEY([ContactID])
REFERENCES [dbo].[Contacts] ([ID])
GO

ALTER TABLE [dbo].[Emails] CHECK CONSTRAINT [FK_Emails_Contacts]
GO

