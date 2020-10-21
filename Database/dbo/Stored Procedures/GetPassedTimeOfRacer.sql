
CREATE PROCEDURE [dbo].[GetPassedTimeOfRacer]
(
	@checkpointId int,
	@racerId int
)
AS
BEGIN
    SET NOCOUNT ON

	declare @endCheckPointTime Datetime2 = (
		SELECT [time]
		from CheckpointPasses 
		where CheckpointId = @checkpointId and RacerId = @racerId
	)

	declare @startTime DATETIME2 = (
				select min([Time]) 
				from CheckpointPasses
				where RacerId = @racerId 
	)

	SELECT DATEDIFF(S, @startTime, @endCheckPointTime)

END