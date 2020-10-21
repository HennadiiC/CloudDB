CREATE TABLE [dbo].[Racers] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (60) NOT NULL,
    [LastName]  VARCHAR (60) CONSTRAINT [DF_Racers_LastName] DEFAULT ('Racer') NOT NULL,
    CONSTRAINT [PK_Racers] PRIMARY KEY CLUSTERED ([Id] ASC)
);



