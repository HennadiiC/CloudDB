CREATE TABLE [dbo].[Checkpoints] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50) NULL,
    [Distance] FLOAT (53)    NOT NULL,
    [RaceId]   INT           NOT NULL,
    CONSTRAINT [PK_Checkpoints] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Checkpoints_Racings] FOREIGN KEY ([RaceId]) REFERENCES [dbo].[Racings] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Checkpoints]
    ON [dbo].[Checkpoints]([Id] ASC);

