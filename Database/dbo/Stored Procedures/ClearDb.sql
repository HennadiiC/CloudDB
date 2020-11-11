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
	delete from Countries
	delete from Seasons

	DBCC CHECKIDENT ('CheckpointPasses', RESEED, 0)  
	DBCC CHECKIDENT ('Racers', RESEED, 0)  
	DBCC CHECKIDENT ('Checkpoints', RESEED, 0)  
	DBCC CHECKIDENT ('Racings', RESEED, 0)  
	DBCC CHECKIDENT ('Countries', RESEED, 0)  
	DBCC CHECKIDENT ('Seasons', RESEED, 0)  
END