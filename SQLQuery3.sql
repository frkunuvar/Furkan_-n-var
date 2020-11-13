USE [WebVeri]
GO

/****** Object:  Table [dbo].[UrunBilgileri]    Script Date: 13.11.2020 02:19:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UrunBilgileri](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UrunAdi] [nvarchar](250) NULL,
	[GoruntulenmeSayisi] [int] NULL,
	[Fiyat] [decimal](18, 2) NULL
) ON [PRIMARY]

GO


