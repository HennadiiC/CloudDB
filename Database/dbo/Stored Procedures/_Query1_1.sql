CREATE PROCEDURE [dbo].[_Query1]
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

	select RacerId, rc.FirstName, rc.LastName, c.Name Country, r.SeasonId, count(Position) QuantityOfRacingsWhereRacerIsInTop8
	from racersPositions rp
	join Racings r on rp.RaceId = r.Id
	join Racers rc on RacerId = rc.Id
	join Countries c on rc.CountryId = c.Id
	where Position <= 8
	group by  rc.FirstName, rc.LastName, r.SeasonId, RacerId, c.Name
	having count(Position) > 3
	order by SeasonId, RacerId, COUNT(Position) desc

END