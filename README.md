# Introduction 
A C#-based testing framework to write and run tests for logic within SQL Server

[![](https://img.shields.io/nuget/v/DBConfirm.Core)](https://www.nuget.org/packages/DBConfirm.Core/)
[![](https://img.shields.io/nuget/dt/DBConfirm.Core)](https://www.nuget.org/packages/DBConfirm.Core/)
[![Build Status](https://dev.azure.com/bungalow64/Bungalow64.SqlTesting/_apis/build/status/Bungalow64.SqlTesting/Sprint-CI)](https://dev.azure.com/bungalow64/Bungalow64.SqlTesting/_build/latest?definitionId=1)

# What is DBConfirm?
DBConfirm is a unit testing framework for SQL databases from within .Net projects.

# Why?
Developers are pretty good at writing unit tests for their application logic already, but sometimes database logic (stored procedures, views, etc.) can be overlooked.  A big reason is that traditionally SQL unit tests are difficult to write, or have a very steep learning curve.  DBConfirm aims to solve this by allowing SQL tests to be written in the same way that all other unit tests are written, so that they are easy to write, easy to maintain, and easy to run.

# How?
The DBConfirm framework is designed to execute tests against a physical instance of the database under test, and ensures that each test run is accurate and repeatable by making sure all effects of a test are rolled back when the test has finished.

# Where's the full documentation?

For the full documentation, see [DBConfirm.bungalow64.co.uk](https://DBConfirm.bungalow64.co.uk/).

# What does a DBConfirm test look like?
A simple test (in MSTest) to call a stored procedure then verify that the data has been added, looks like this:

```csharp
[TestMethod]
public async Task AddUserProcedure_UserIsAdded()
{
    // Call a stored procedure with some parameters
    await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
        new SqlQueryParameter("FirstName", "Sarah"),
        new SqlQueryParameter("LastName", "Connor"),
        new SqlQueryParameter("EmailAddress", "sarah@example.com"));

    // Get all the data in a table
    QueryResult data = await TestRunner.ExecuteTableAsync("dbo.Users");

    // Make some assertions on the data
    data
        .AssertRowCount(1) // Asserts that there is only 1 row
        .AssertValue(0, "FirstName", "Sarah"); // Asserts that the "FirstName" value is "Sarah" in the first row
}
```

# Getting started

Install the NuGet package for the test framework you're currently using, either MSTest or NUnit:

* Install-Package [DBConfirm.Packages.SQLServer.MSTest](https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.MSTest/)
* Install-Package [DBConfirm.Packages.SQLServer.NUnit](https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.NUnit/)

In the root of your test project, add an appsettings.json file with the connection string to the database to be tested:

```json
{
  "ConnectionStrings": {
    "TestDatabase": "SERVER=(local);DATABASE=TestDB;Integrated Security=true;Connection Timeout=30;"
  }
}
```

> Note, this **appsettings.json** file needs to be set to be copied to the output directory.

Add a new test file, and inherit from the base class for the framework (either `MSTestBase` or `NUnitBase`):

```csharp
// For MSTest
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBConfirm.Packages.SQLServer.MSTest;

[TestClass]
public class GetUserTests : MSTestBase
{
    ...
}
```

> For NUnit, use `[TestFixture]` instead of `[TestClass]` and inherit from `DBConfirm.Packages.SQLServer.NUnit.NUnitBase` instead of `MSTestBase`.

Add a new test method, and start testing:

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

For the full documentation, see [DBConfirm.bungalow64.co.uk](https://DBConfirm.bungalow64.co.uk/).