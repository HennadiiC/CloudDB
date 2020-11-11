CREATE PROCEDURE [dbo].[_Query3]
AS
BEGIN
    SET NOCOUNT ON

	/*
	
	Для страны вывести среднее время прохождения гонок
	
	*/

	;with personPassing as (
		SELECT c.Id countryId, cp.RaceId, r.Id racerId, DATEDIFF(SECOND, min(Time), max(time)) RacePassingTime
		from CheckpointPasses cpp
		join Racers r on cpp.RacerId = r.Id
		join Countries c on r.CountryId = c.Id
		join Checkpoints cp on cpp.CheckpointId = cp.Id
		group by c.Id, cp.RaceId, r.Id
	)

    select c.Name, AVG(RacePassingTime) avgRacePassingForCountry
	from personPassing
	join Countries c on countryId = c.id
	group by c.Name
	order by avgRacePassingForCountry

END