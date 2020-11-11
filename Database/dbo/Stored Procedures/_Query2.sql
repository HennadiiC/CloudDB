CREATE PROCEDURE [dbo].[_Query2]
(
	@countryId int
)
AS
BEGIN
    SET NOCOUNT ON

	/*

	Для гонщиков из заданной страны вывести гонку и время прохода

	*/

    SELECT RacerId, RaceId, DATEDIFF(SECOND, Min(Time), max(time)) TimeInSeconds
	from CheckpointPasses cpp
	join Checkpoints cp on cpp.CheckpointId = cp.Id
	join Racers r on cpp.RacerId = r.Id
	where r.CountryId = @countryId
	group by RacerId, RaceId

END