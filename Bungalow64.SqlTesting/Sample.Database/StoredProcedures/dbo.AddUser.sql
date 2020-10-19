CREATE PROCEDURE [dbo].[AddUser]
	@FirstName NVARCHAR(50)
	,@LastName NVARCHAR(50)
	,@EmailAddress NVARCHAR(50)
	,@StartDate DATETIME
	,@NumberOfHats INT
	,@Cost DECIMAL(18,2)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO dbo.Users
	(FirstName, LastName, EmailAddress, CreatedDate, StartDate, NumberOfHats, Cost)
	VALUES
	(@FirstName, @LastName, @EmailAddress, GETUTCDATE(), @StartDate, @NumberOfHats, @Cost)
END
GO


