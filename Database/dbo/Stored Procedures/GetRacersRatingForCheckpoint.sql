-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetRacersRatingForCheckpoint]
(
	@checkPointId int
)
AS
BEGIN
    SET NOCOUNT ON

    declare @raceId int = (select RaceId from Checkpoints where Id = @checkPointId)

	select RacerId, min([Time]), max([Time])
	from CheckpointPasses cpp
	join Checkpoints cp on cp.Id = cpp.CheckpointId
	where RaceId = @raceId and CheckpointId <= @checkPointId
	group by RacerId
	order by DATEDIFF(ms, min([Time]), max([Time]))

END