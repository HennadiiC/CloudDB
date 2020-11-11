
CREATE FUNCTION [dbo].[reduce3]
(
	@input table3 readonly
)
RETURNS 
@res TABLE 
(
	Name nvarchar(60), 
	avgRacePassingForCountry int
)
AS
BEGIN
	insert into @res
	select c.Name, AVG(RacePassingTime) avgRacePassingForCountry
	from @input
	join Countries c on countryId = c.id
	group by c.Name
	OPTION(USE HINT('ENABLE_PARALLEL_PLAN_PREFERENCE'))
	
	RETURN 
END