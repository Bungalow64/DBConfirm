
DECLARE @ActualName NVARCHAR(255)
SELECT @ActualName = Name FROM master.dbo.sysdatabases WHERE Name = @DatabaseName

IF (@ActualName IS NOT NULL)
BEGIN
	EXEC('DROP DATABASE ' + @ActualName)
END