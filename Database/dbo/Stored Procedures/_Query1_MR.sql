CREATE PROCEDURE [dbo].[_Query1_MR]
AS
BEGIN
    SET NOCOUNT ON
	
	declare @input table1
	insert into @input
	select * from dbo.map1()

	select RacerId,  FirstName, LastName, Country, SeasonId, QuantityOfRacingsWhereRacerIsInTop8
	from dbo.reduce1(@input) rp

END