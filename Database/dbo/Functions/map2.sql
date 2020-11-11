CREATE FUNCTION [dbo].[map2]
(
	@countryId int
)
RETURNS 
@res TABLE 
(
	RacerId int,
	RaceId int,
	Time datetime2
)
AS
BEGIN
	insert into @res
	SELECT RacerId, RaceId, Time
	from CheckpointPasses cpp
	join Checkpoints cp on cpp.CheckpointId = cp.Id
	join Racers r on cpp.RacerId = r.Id
	where r.CountryId = @countryId
	OPTION(USE HINT('ENABLE_PARALLEL_PLAN_PREFERENCE'))
	
	RETURN 
END