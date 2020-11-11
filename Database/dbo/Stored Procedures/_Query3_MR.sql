CREATE PROCEDURE [dbo].[_Query3_MR]
AS
BEGIN
    SET NOCOUNT ON

    declare @input table3
	insert into @input
	select * from dbo.map3()

	select *
	from dbo.reduce3(@input) rp
END