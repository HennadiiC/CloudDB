CREATE PROCEDURE [dbo].[_Query2_MR]
(
	@countryId int
)
AS
BEGIN
    SET NOCOUNT ON

    declare @input table2
	insert into @input
	select * from dbo.map2(@countryId)

	select *
	from dbo.reduce2(@input) rp
END