# Introduction 
A C#-based testing framework to write and run tests for logic within SQL Server

[![](https://img.shields.io/nuget/v/DBConfirm.Core)](https://www.nuget.org/packages/DBConfirm.Core/)
[![](https://img.shields.io/nuget/dt/DBConfirm.Core)](https://www.nuget.org/packages/DBConfirm.Core/)
[![Build Status](https://dev.azure.com/bungalow64/Bungalow64.SqlTesting/_apis/build/status/Master-CI%20(GitHub)?branchName=master)](https://dev.azure.com/bungalow64/Bungalow64.SqlTesting/_build/latest?definitionId=11&branchName=master)
[![Repo Size](https://img.shields.io/github/repo-size/bungalow64/dbconfirm)](https://github.com/Bungalow64/DBConfirm)
[![Licence](https://img.shields.io/github/license/bungalow64/dbconfirm)](https://github.com/Bungalow64/DBConfirm)
[![Release Date](https://img.shields.io/github/release-date/bungalow64/dbconfirm?label=latest%20release)](https://github.com/Bungalow64/DBConfirm)

# What is DBConfirm?
DBConfirm is a unit testing framework for SQL Server databases from within .Net projects.  Tests can be written to check that stored procedures and views behave as you'd expect, and can be used to help reduce the number of bugs introduced.  DBConfirm also provides patterns and tools to easily set up prerequisite data needed for your tests.

# Why?
Developers are pretty good at writing unit tests for their application logic already, but sometimes database logic (stored procedures, views, etc.) can be overlooked.  A big reason is that traditionally SQL unit tests are difficult to write, or have a very steep learning curve.  DBConfirm aims to solve this by allowing SQL tests to be written in the same way that all other unit tests are written, so that they are easy to write, easy to maintain, and easy to run.

# How?
The DBConfirm framework is designed to execute tests against a physical instance of the database under test, and ensures that each test run is accurate and repeatable by making sure all effects of a test are rolled back when the test has finished.

# Where's the full documentation?

For the full documentation, see [dbconfirm.com](https://dbconfirm.com/).

# What versions of SQL Server does DBConfirm work with?
DBConfirm is compatible with SQL Server 2014, 2016, 2017, 2019 and with Azure SQL Database.

# What does a DBConfirm test look like?
A simple test (in MSTest) to call a stored procedure then verify that the data has been added, looks like this:

```csharp
[TestMethod]
public async Task AddUserProcedure_UserIsAdded()
{
    // Call a stored procedure with some parameters
    await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser", new DataSetRow
    {
        ["FirstName"] = "Sarah",
        ["LastName"] = "Connor",
        ["EmailAddress"] = "sarah@example.com"
    });

    // Get all the data in a table
    QueryResult data = await TestRunner.ExecuteTableAsync("dbo.Users");

    // Make some assertions on the data
    data
        .AssertRowCount(1) // Asserts that there is only 1 row
        .AssertValue(0, "FirstName", "Sarah"); // Asserts that "FirstName" is "Sarah" in the first row
}
```

# Getting started

The quickest way to get started is to use a DBConfirm project template.

Install the template for MSTest ([DBConfirm.Templates.SQLServer.MSTest](https://www.nuget.org/packages/DBConfirm.Templates.SQLServer.MSTest/)) by executing this command:

```powershell
dotnet new -i DBConfirm.Templates.SQLServer.MSTest
```

> Alternatively, you can use the [DBConfirm.Templates.SQLServer.NUnit](https://www.nuget.org/packages/DBConfirm.Templates.SQLServer.NUnit/) template to use the NUnit test framework

Once installed, create a new DBConfirm project by executing this command:

```powershell
dotnet new dbconfirm-sqlserver-mstest -n "YourProjectName"
```

> Alternatively, for NUnit, execute dbconfirm-sqlserver-nunit

The project will then be added, with a sample unit test class in the root.

You'll need to update the config file to have a correct connection string in there, pointing to the database you want to test.  In the root of your test project, find the appsettings.json file and update the DefaultConnectionString value:

```json
{
  "ConnectionStrings": {
    "DefaultConnectionString": "SERVER=(local);DATABASE=TestDB;Integrated Security=true;Connection Timeout=30;"
  }
}
```

You're now ready to start testing.  Add a new test method, and start testing:

```csharp
// For MSTest
[TestMethod]
public async Task GetUsersView_ContainsFirstNameColumn()
{
    var results = await TestRunner.ExecuteTableAsync("dbo.GetUsers");

    results
        .AssertColumnExists("FirstName");
}
```

> For NUnit, use `[Test]` instead of `[TestMethod]`.

# The fundamentals of a DBConfirm test

## Arrange - set up any prerequisite test data

Data can be inserted into any table in the database using either the DBConfirm API or by creating templates that are used to set up complex scenarios across multiple tests.

## Act - run the SQL you want to test

Using the DBConfirm API you can trigger any stored procedure, query any view, or run any arbitrary SQL statement.

## Assert - check the returned data, and check the state of data in the database

The DBConfirm API provides a whole bunch of assertion methods that you can run on the returned data, including checking specific columns are present, that values are what you expect them to be, and that at least one row meets your conditions, etc.

You can also use the DBConfirm API to query the data in tables, and run the same assertions, to make sure the data is in the exact state that you expect.

For the full documentation, see [DBConfirm.com](https://dbconfirm.com/).

# Buliding and running the source of DBConfirm

Feel free to fork/clone this repository.  Once you have the source files locally, open DBConfirm.sln, and the solution will load.  Each NuGet package is its own project; hopefully you will be able to find your way around.

There are a number of test projects, and these fall under 2 categories:
- unit tests
- sample project tests

The unit test projects are all within the 5.Tests solution folder.  These are just standard unit tests, nothing special there.

The sample project tests, within the 6.SampleSolutions solution foder, are more like integration tests, that carry out DBConfirm tests against an actual database.

## Running sample project tests

These tests require an actual database to test against, so for these tests to run, you'll need to set these databases up.

The easiest/quickest way is to use [Docker](https://www.docker.com/).  Once you have Docker installed and running locally, open up your command prompt, go to the root of the DBConfirm solution, and run these commands:

```powershell
docker compose build
docker compose up
```

This will set up the databases for you, and host them in SQL 2017 and 2019.  The tests in the sample solutions are already set up to connect to these databases, so you should be able to just run all those tests, and they'll all (hopefully) pass.

If you want to access these databases directly, they are located here:
- Server: localhost
- Port: 1401 (for SQL 2017), 1402 (for SQL 2019)
- Databases: SampleDB, Northwind
- Credentials: For the 'sa' password, check the contents of the docker\sqlserver.env file.


The continuous integration builds we have set up also use Docker to set up the test environment, so we can be assured that everything is working as expected.

Alternatively, if you don't want to use Docker, you can host the databases directly.  The setup scripts for the databases are within 6.SampleSolutions\1.Databases.  Run these on an instance of SQL somewhere, and update the connection strings in the test projects (such as 6.SampleSolutions\2.ProjectTests\Sample.Core.MSTest.Tests\appsettings.json) to get them up and running.

