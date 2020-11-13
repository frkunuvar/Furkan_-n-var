USE [WebVeri]
GO

/****** Object:  Table [dbo].[Parametreler]    Script Date: 13.11.2020 02:19:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Parametreler](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[DegiskenAdi] [nvarchar](50) NULL,
	[Deger] [nvarchar](50) NULL
) ON [PRIMARY]

GO


