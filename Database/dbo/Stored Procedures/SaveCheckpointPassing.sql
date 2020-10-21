CREATE PROCEDURE SaveCheckpointPassing
(
    @checkpointId INT,
	@racerId INT,
	@passTime DATETIME2
)
AS
BEGIN
    SET NOCOUNT ON

	INSERT INTO CheckpointPasses (CheckpointId, RacerId, Time)
	VALUES (@checkpointId, @racerId, @passTime)
END