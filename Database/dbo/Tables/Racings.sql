CREATE TABLE [dbo].[Racings] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Racings] PRIMARY KEY CLUSTERED ([Id] ASC)
);

