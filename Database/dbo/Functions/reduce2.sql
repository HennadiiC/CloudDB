
CREATE FUNCTION [dbo].[reduce2] 
(
	@input table2 readonly
)
RETURNS 
@res TABLE 
(
	RacerId int, 
	RaceId int, 
	TimeInSeconds int
)
AS
BEGIN
	insert into @res
	select RacerId, RaceId, DATEDIFF(SECOND, Min(Time), max(time)) TimeInSeconds
	from @input
	group by RacerId, RaceId
	OPTION(USE HINT('ENABLE_PARALLEL_PLAN_PREFERENCE'))
	
	RETURN 
END