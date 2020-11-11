
CREATE FUNCTION [dbo].[map3]
(
)
RETURNS 
@res TABLE 
(
	countryId int,
	RaceId int,
	racerId int,
	RacePassingTime int
)
AS
BEGIN
	insert into @res
	SELECT c.Id countryId, cp.RaceId, r.Id racerId, DATEDIFF(SECOND, min(Time), max(time)) RacePassingTime
		from CheckpointPasses cpp
		join Racers r on cpp.RacerId = r.Id
		join Countries c on r.CountryId = c.Id
		join Checkpoints cp on cpp.CheckpointId = cp.Id
		group by c.Id, cp.RaceId, r.Id
		OPTION(USE HINT('ENABLE_PARALLEL_PLAN_PREFERENCE'))
	
	RETURN 
END