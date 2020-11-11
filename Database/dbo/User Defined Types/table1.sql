CREATE TYPE [dbo].[table1] AS TABLE (
    [RacerId]  INT    NOT NULL,
    [RaceId]   INT    NOT NULL,
    [Position] BIGINT NULL,
    PRIMARY KEY CLUSTERED ([RacerId] ASC, [RaceId] ASC));

