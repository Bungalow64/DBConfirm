CREATE PROCEDURE [dbo].[CountUsers]
	@EmailAddress NVARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		COUNT(*) AS TotalUsers
	FROM
		dbo.Users
	WHERE
		EmailAddress = @EmailAddress
END
GO


