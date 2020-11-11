CREATE PROCEDURE dbo._Query1
AS
BEGIN
    SET NOCOUNT ON

    ;with racersPositions as (
		select 
			RacerId, 
			RaceId,
			ROW_NUMBER() OVER (PARTITION BY RaceId order by DATEDIFF(SECOND, min(Time), MAX(time))) Position
		from CheckpointPasses cpp
		join Checkpoints cp on cpp.CheckpointId = cp.Id

		group by RacerId, RaceId
	)

	select r.SeasonId, RacerId, count(Position) QuantityOfRacingsWhereRacerIsInTop8
	from racersPositions rp
	join Racings r on rp.RaceId = r.Id
	where Position <= 8
	group by r.SeasonId, RacerId
	having count(Position) > 3

END