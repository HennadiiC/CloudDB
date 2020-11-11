CREATE TYPE [dbo].[table3] AS TABLE (
    [countryId]       INT NULL,
    [RaceId]          INT NOT NULL,
    [racerId]         INT NOT NULL,
    [RacePassingTime] INT NULL,
    PRIMARY KEY CLUSTERED ([RaceId] ASC, [racerId] ASC));

