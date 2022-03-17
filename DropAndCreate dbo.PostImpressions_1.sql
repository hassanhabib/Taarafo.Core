USE [TaarafoCoreDb]
GO

/****** Object: Table [dbo].[PostImpressions] Script Date: 3/16/2022 11:03:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[PostImpressions];


GO
CREATE TABLE [dbo].[PostImpressions] (
    [PostId]      UNIQUEIDENTIFIER   NOT NULL,
    [ProfileId]   UNIQUEIDENTIFIER   NOT NULL,
    [CurrentDate] DATETIMEOFFSET (7) NOT NULL,
    [UpdateDate]  DATETIMEOFFSET (7) NOT NULL,
    [Impression]  INT                NOT NULL
);


