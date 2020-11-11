
CREATE FUNCTION [dbo].[reduce1]
(
	@input table1 readonly
)
RETURNS 
@t TABLE 
(
	RacerId int,
	FirstName nvarchar(60),
	LastName nvarchar(60),
	Country nvarchar(60),
	SeasonId int, 
	QuantityOfRacingsWhereRacerIsInTop8 int
)
AS
BEGIN
	insert into @t (RacerId, FirstName, LastName, Country, SeasonId, QuantityOfRacingsWhereRacerIsInTop8)
	
	select RacerId,  rc.FirstName, rc.LastName, c.Name Country, r.SeasonId, count(Position) QuantityOfRacingsWhereRacerIsInTop8
	from @input rp
	join Racings r on rp.RaceId = r.Id
	join Racers rc on RacerId = rc.Id
	join Countries c on rc.CountryId = c.Id

	where Position <= 8
	group by r.SeasonId, RacerId,  rc.FirstName,  rc.LastName, c.Name
	having count(Position) > 3
	OPTION(USE HINT('ENABLE_PARALLEL_PLAN_PREFERENCE'))
	
	RETURN 
END