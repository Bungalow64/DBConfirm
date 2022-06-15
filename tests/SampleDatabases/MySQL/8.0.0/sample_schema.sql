SET NAMES utf8mb4;
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';

DROP SCHEMA IF EXISTS SampleDB;
CREATE SCHEMA SampleDB;
USE SampleDB;

CREATE TABLE Users(
	`Id` int AUTO_INCREMENT NOT NULL,
	`FirstName` varchar(50) NOT NULL,
	`LastName` varchar(50) NOT NULL,
	`EmailAddress` varchar(50) NOT NULL,
	`CreatedDate` datetime(3) NOT NULL,
	`StartDate` datetime(3) NOT NULL,
	`IsActive` Tinyint NOT NULL DEFAULT 1,
	`NumberOfHats` bigint NOT NULL,
	`HatType` varchar(50) NULL,
	`Cost` decimal(18, 2) NOT NULL,
	PRIMARY KEY  (Id)
);

CREATE VIEW AllUsers
AS
SELECT * FROM Users;

CREATE TABLE Countries(
	`CountryCode` varchar(50) NOT NULL,
	`CountryName` varchar(50) NOT NULL,
 CONSTRAINT `PK_Countries` PRIMARY KEY 
(
	`CountryCode` ASC
) 
);

CREATE TABLE NumbersTable(
	`Id` int AUTO_INCREMENT NOT NULL,
	`IntColumn` int NULL,
	`SmallIntColumn` smallint NULL,
	`BigIntColumn` bigint NULL,
	`DecimalColumn` decimal(18, 0) NULL,
	`MoneyColumn` Decimal(15,4) NULL,
	`SmallMoneyColumn` Decimal(6,4) NULL,
	`NumericColumn` numeric(18, 0) NULL,
	`FloatColumn` Double NULL,
	`RealColumn` real NULL,
	`TinyIntColumn` tinyint Unsigned NULL,
	PRIMARY KEY  (Id)
);

CREATE TABLE UserAddresses(
	`Id` int AUTO_INCREMENT NOT NULL,
	`UserId` int NOT NULL,
	`Address1` varchar(50) NOT NULL,
	`Postcode` varchar(10) NOT NULL,
	`Other` varchar(50) NULL,
	`CountryCode` varchar(50) NULL,
	PRIMARY KEY  (Id),
	CONSTRAINT FK_UserAddresses_Countries FOREIGN KEY (CountryCode) REFERENCES Countries (CountryCode) ON DELETE RESTRICT ON UPDATE CASCADE,
	CONSTRAINT FK_UserAddresses_Users FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE RESTRICT ON UPDATE CASCADE
);

DELIMITER //

CREATE PROCEDURE AddUser (
	IN p_FirstName VARCHAR(50)
	,IN p_LastName VARCHAR(50)
	,IN p_EmailAddress VARCHAR(50)
	,IN p_StartDate DATETIME(3)
	,IN p_NumberOfHats INT
	,IN p_Cost DECIMAL(18,2))
BEGIN

    INSERT INTO Users
	(FirstName, LastName, EmailAddress, CreatedDate, StartDate, NumberOfHats, Cost)
	VALUES
	(p_FirstName, p_LastName, p_EmailAddress, UTC_TIMESTAMP(3), p_StartDate, p_NumberOfHats, p_Cost);
END//

CREATE PROCEDURE CountUsers (
	IN p_EmailAddress VARCHAR(50))
READS SQL DATA
BEGIN

	SELECT
		COUNT(*) AS TotalUsers
	FROM
		Users
	WHERE
		EmailAddress = p_EmailAddress;
END//

CREATE PROCEDURE GetUser (
	IN p_EmailAddress VARCHAR(50))
READS SQL DATA
BEGIN

    SELECT
		FirstName
		,LastName
		,UserAddresses.Postcode
	FROM
		Users
	LEFT JOIN
		UserAddresses ON Users.Id = UserAddresses.UserId
	WHERE
		EmailAddress = p_EmailAddress;

	SELECT
		COUNT(*) AS TotalUsers
	FROM
		Users
	WHERE
		EmailAddress = p_EmailAddress;
END//


DELIMITER ;

CREATE TABLE IdentityOnlyTable(
	Id int AUTO_INCREMENT NOT NULL,
	PRIMARY KEY  (Id)
);

CREATE TABLE `User's`(
	Id int AUTO_INCREMENT NOT NULL,
	Name varchar(50) NULL,
	PRIMARY KEY  (Id)
);

CREATE TABLE FXC15NX9HYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLI(
	Id int AUTO_INCREMENT NOT NULL,
	Name varchar(50) NULL,
	PRIMARY KEY  (Id)
);

CREATE TABLE NoPrimaryKeyTable(
	Id int NULL
);