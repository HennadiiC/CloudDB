CREATE TABLE [dbo].[CheckpointPasses] (
    [RacerId]              INT           NOT NULL,
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [CheckpointId]         INT           NOT NULL,
    [Time]                 DATETIME2 (7) NOT NULL,
    [DistanceToCheckPoint] FLOAT (53)    CONSTRAINT [DF_CheckpointPasses_DistanceToCheckPoint] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CheckpointPasses] PRIMARY KEY CLUSTERED ([RacerId] ASC, [Id] ASC) ON [PS_RACER_BY_ID] ([RacerId]),
    CONSTRAINT [FK_Checkpointpasses_Checkpoints] FOREIGN KEY ([CheckpointId]) REFERENCES [dbo].[Checkpoints] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Checkpointpasses_Racers] FOREIGN KEY ([RacerId]) REFERENCES [dbo].[Racers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
) ON [PS_RACER_BY_ID] ([RacerId]);





