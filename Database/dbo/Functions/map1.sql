CREATE function [dbo].[map1]
(
)
returns 
@t TABLE ( 
	RacerId int,
	RaceId int,
	Position bigint 
)
WITH EXECUTE AS CALLER
AS 
begin
	insert into @t 
	
	
	select 
			RacerId, 
			RaceId,
			ROW_NUMBER() OVER (PARTITION BY RaceId order by DATEDIFF(SECOND, min(Time), MAX(time))) Position
	from CheckpointPasses cpp
	join Checkpoints cp on cpp.CheckpointId = cp.Id

	group by RacerId, RaceId
	OPTION(USE HINT('ENABLE_PARALLEL_PLAN_PREFERENCE'))

	Return
end