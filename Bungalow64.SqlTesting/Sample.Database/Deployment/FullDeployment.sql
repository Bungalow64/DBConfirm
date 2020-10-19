CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[NumberOfHats] [bigint] NOT NULL,
	[HatType] [nvarchar](50) NULL,
	[Cost] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

CREATE TABLE [dbo].[UserAddresses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Address1] [nvarchar](50) NOT NULL,
	[Postcode] [nvarchar](10) NOT NULL,
	[Other] [nvarchar](50) NULL,
 CONSTRAINT [PK_UserAddresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserAddresses]  WITH CHECK ADD  CONSTRAINT [FK_UserAddresses_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[UserAddresses] CHECK CONSTRAINT [FK_UserAddresses_Users]
GO

CREATE VIEW [dbo].[AllUsers]
AS
SELECT * FROM dbo.Users
GO


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