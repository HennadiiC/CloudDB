-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetRacerSpeedForCheckPoint]
(
	@racerId int,
	@checkpointId int
)
AS
BEGIN

    SET NOCOUNT ON

	declare @endCheckPointTime Datetime2 = (
		SELECT [Time] 
		FROM CheckpointPasses
		where RacerId = @racerId and CheckpointId = @checkpointId
	)
 
	declare @startPointTime Datetime2 = (
		SELECT max([Time]) 
		FROM CheckpointPasses
		where RacerId = @racerId and [Time] < @endCheckPointTime
	)

	declare @distance float = (SELECT Distance FROM Checkpoints WHERE ID = @checkpointId);

	Select @distance/DATEDIFF(S, @startPointTime, @endCheckPointTime)

END