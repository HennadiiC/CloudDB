-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetPassedDistanceForRacer]
(
	@racerId int,
	@raceId int
)
AS
BEGIN

    SET NOCOUNT ON

	SELECT SUM(Checkpoints.Distance) as TotalDistance 
	FROM Checkpoints 
	JOIN CheckpointPasses ON CheckpointPasses.CheckpointId = Checkpoints.Id
	JOIN Racers ON Racers.Id = CheckpointPasses.RacerId 
	JOIN Racings ON Racings.Id = Checkpoints.RaceId 
	Where Racers.Id = @racerId AND Racings.Id = @raceId
	
END