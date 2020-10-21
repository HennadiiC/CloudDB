CREATE TABLE [dbo].[Racers] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [FullName] VARCHAR (60) NOT NULL,
    CONSTRAINT [PK_Racers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

