-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[ClearDb]
AS
BEGIN
    SET NOCOUNT ON

    delete from CheckpointPasses
	delete from Racers
	delete from Checkpoints
	delete from Racings

	DBCC CHECKIDENT ('CheckpointPasses', RESEED, 1)  
	DBCC CHECKIDENT ('Racers', RESEED, 1)  
	DBCC CHECKIDENT ('Checkpoints', RESEED, 1)  
	DBCC CHECKIDENT ('Racings', RESEED, 1)  
END