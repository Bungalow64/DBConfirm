CREATE PROCEDURE [dbo].[GetUser]
	@EmailAddress NVARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;

    SELECT
		FirstName
		,LastName
		,UserAddresses.Postcode
	FROM
		dbo.Users
	LEFT JOIN
		dbo.UserAddresses ON Users.Id = UserAddresses.UserId
	WHERE
		EmailAddress = @EmailAddress

	SELECT
		COUNT(*) AS TotalUsers
	FROM
		dbo.Users
	WHERE
		EmailAddress = @EmailAddress
END
GO