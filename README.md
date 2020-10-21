# Introduction 
A C#-based testing framework to write and run tests for logic within SQL Server

[![](https://img.shields.io/nuget/v/SQLConfirm.Core)](https://www.nuget.org/packages/SQLConfirm.Core/)
[![](https://img.shields.io/nuget/dt/SQLConfirm.Core)](https://www.nuget.org/packages/SQLConfirm.Core/)
[![Build Status](https://dev.azure.com/bungalow64/Bungalow64.SqlTesting/_apis/build/status/Bungalow64.SqlTesting/Sprint-CI)](https://dev.azure.com/bungalow64/Bungalow64.SqlTesting/_build/latest?definitionId=1)

# What is SQLConfirm?
SQLConfirm is a unit testing framework for SQL databases from within .Net projects.

# Why?
Developers are pretty good at writing unit tests for their application logic already, but sometimes database logic (stored procedures, views, etc.) can be overlooked.  A big reason is that traditionally SQL unit tests are difficult to write, or have a very steep learning curve.  SQLConfirm aims to solve this by allowing SQL tests to be written in the same way that all other unit tests are written, so that they are easy to write, easy to maintain, and easy to run.

# How?
The SQLConfirm framework is designed to execute tests against a physical instance of the database under test, and ensures that each test run is accurate and repeatable by making sure all effects of a test are rolled back when the test has finished.

# What does a SQLConfirm test look like?
A simple test (in MSTest) to call a stored procedure then verify that the data has been added, looks like this:

```csharp
[TestMethod]
public async Task AddUserProcedure_UserIsAdded()
{
    // Call a stored procedure with some parameters
    await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
        new SqlQueryParameter("FirstName", "Jamie"),
        new SqlQueryParameter("LastName", "Burns"),
        new SqlQueryParameter("EmailAddress", "jamie@example.com"));

    // Get all the data in a table
    QueryResult data = await TestRunner.ExecuteTableAsync("dbo.Users");

    // Make some assertions on the data
    data
        .AssertRowCount(1) // Asserts that there is only 1 row
        .AssertValue(0, "FirstName", "Jamie"); // Asserts that the "FirstName" value is "Jamie" in the first row
}
```

# Getting started

Install the NuGet package for the test framework you're currently using, either MSTest or NUnit:

* Install-Package [SQLConfirm.Packages.SQLServer.MSTest](https://www.nuget.org/packages/SQLConfirm.Packages.SQLServer.MSTest/)
* Install-Package [SQLConfirm.Packages.SQLServer.NUnit](https://www.nuget.org/packages/SQLConfirm.Packages.SQLServer.NUnit/)

In the root of your test project, add an appsettings.json file with the connection string to the database to be tested:

```json
{
  "ConnectionStrings": {
    "TestDatabase": "SERVER=(local);DATABASE=TestDB;Integrated Security=true;Connection Timeout=30;"
  }
}
```

Add a new test file, and inherit from the SQLConfirmTestBase class:

```csharp
// For MSTest
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLConfirm.Packages.SQLServer.MSTest;

[TestClass]
public class GetUserTests : MSTestBase
{
    ...
}
```

```csharp
// For NUnit
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLConfirm.Packages.SQLServer.NUnit;

[TestFixture]
public class GetUserTests : NUnitBase
{
    ...
}
```

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

```csharp
// For NUnit
[Test]
public async Task GetUsersView_ContainsFirstNameColumn()
{
    var results = await TestRunner.ExecuteTableAsync("dbo.GetUsers");

    results
        .AssertColumnExists("FirstName");
}
```

# The fundamentals of a SQLConfirm test

## Arrange - set up any prerequisite test data

Data can be inserted into any table in the database using either the SQLConfirm API or by creating templates that are used to set up complex scenarios across multiple tests.

## Act - run the SQL you want to test

Using the SQLConfirm API you can trigger any stored procedure, query any view, or run any arbitrary SQL statement.

## Assert - check the returned data, and check the state of data in the database

The SQLConfirm API provides a whole bunch of assertion methods that you can run on the returned data, including checking specific columns are present, that values are what you expect them to be, and that at least one row meets your conditions, etc.

You can also use the SQLConfirm API to query the data in tables, and run the same assertions, to make sure the data is in the exact state that you expect.

# The SQLConfirm API

The SQLConfirm API is accessed via the `TestRunner` property, on the test's base class.  Most of the API methods are awaitable, so make sure your test is marked as **async** and return a `Task`.

## `CountRowsInTableAsync`

Returns the total number of rows in the table

```csharp
Task<int> CountRowsInTableAsync(string tableName)
```

Parameters:
* **tableName** - the name of the table, including schema

Returns:
* the total number of rows

## `CountRowsInViewAsync`

Returns the total number of rows in the view

```csharp
Task<int> CountRowsInViewAsync(string viewName)
```

Parameters:
* **viewName** - the name of the view, including schema

Returns:
* the total number of rows

## `ExecuteCommandAsync`

Executes a command, returning a single data table

```csharp
Task<QueryResult> ExecuteCommandAsync(string commandText, IDictionary<string, object> parameters)

Task<QueryResult> ExecuteCommandAsync(string commandText, params SqlQueryParameter[] parameters)
```

Parameters:
* **commandText** - The command to execute
* **parameters** - The parameters to be used (when an IDictionary is used, the Key is used as the parameter name, and the Value used as the parameter value)

Returns:
* Where the command returns a data set, the first table is returned, otherwise an empty data set is returned

## `ExecuteCommandMultipleDataSetAsync`

Executes a command, returning all data tables

```csharp
Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, IDictionary<string, object> parameters)

Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, params SqlQueryParameter[] parameters)
```

Parameters:
* **commandText** - The command to execute
* **parameters** - The parameters to be used (when an IDictionary is used, the Key is used as the parameter name, and the Value used as the parameter value)

Returns:
* Returns all tables returned from the command

## `ExecuteCommandNoResultsAsync`

Executes a command, returning no data

```csharp
Task ExecuteCommandNoResultsAsync(string commandText, IDictionary<string, object> parameters)

Task ExecuteCommandNoResultsAsync(string commandText, params SqlQueryParameter[] parameters)
```

Parameters:
* **commandText** - The command to execute
* **parameters** - The parameters to be used (when an IDictionary is used, the Key is used as the parameter name, and the Value used as the parameter value)

Returns:
* Nothing

## `ExecuteCommandScalarAsync<T>`

Executes a command, returning a single object

```csharp
Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, IDictionary<string, object> parameters)

Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, params SqlQueryParameter[] parameters)
```

Types:
* **T** - The type of the object to return

Parameters:
* **commandText** - The command to execute
* **parameters** - The parameters to be used (when an IDictionary is used, the Key is used as the parameter name, and the Value used as the parameter value)

Returns:
* Returns the object returned from the command

## `ExecuteStoredProcedureMultipleDataSetAsync`

Executes a stored procedure, returning all data tables

```csharp
Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, IDictionary<string, object> parameters)

Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, params SqlQueryParameter[] parameters)
```

Parameters:
* **procedureName** - The name of the stored procedure, including schema
* **parameters** - The parameters to be used (when an IDictionary is used, the Key is used as the parameter name, and the Value used as the parameter value)

Returns:
* Returns all tables returned from the stored procedure

## `ExecuteStoredProcedureNonQueryAsync`

Executes a stored procedure, returning nothing

```csharp
Task ExecuteStoredProcedureNonQueryAsync(string procedureName, IDictionary<string, object> parameters)

Task ExecuteStoredProcedureNonQueryAsync(string procedureName, params SqlQueryParameter[] parameters)
```

Parameters:
* **procedureName** - The name of the stored procedure, including schema
* **parameters** - The parameters to be used (when an IDictionary is used, the Key is used as the parameter name, and the Value used as the parameter value)

Returns:
* Nothing

## `ExecuteStoredProcedureQueryAsync`

Executes a stored procedure, returning a single data table

```csharp
Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, IDictionary<string, object> parameters)

Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, params SqlQueryParameter[] parameters)
```

Parameters:
* **procedureName** - The name of the stored procedure, including schema
* **parameters** - The parameters to be used (when an IDictionary is used, the Key is used as the parameter name, and the Value used as the parameter value)

Returns:
* Where the stored procedure returns a data set, the first table is returned, otherwise an empty data set is returned

## `ExecuteStoredProcedureScalarAsync<T>`

Executes a stored procedure, returning a single object

```csharp
Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, IDictionary<string, object> parameters)

Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, params SqlQueryParameter[] parameters)
```

Types:
* **T** - The type of the object to return

Parameters:
* **procedureName** - The name of the stored procedure, including schema
* **parameters** - The parameters to be used (when an IDictionary is used, the Key is used as the parameter name, and the Value used as the parameter value)

Returns:
* Returns the object returned from the stored procedure

## `ExecuteTableAsync`

Returns all data for a specific table

```csharp
Task<QueryResult> ExecuteTableAsync(string tableName)
```

Parameters:
* **tableName** - The name of the table, including schema

Returns:
* Returns all columns and values found

## `ExecuteViewAsync`

Returns all data for a specific view

```csharp
Task<QueryResult> ExecuteViewAsync(string viewName)
```

Parameters:
* **viewName** - The name of the view, including schema

Returns:
* Returns all columns and values found

## `InsertDataAsync`

Inserts data into a table.

Where the table has an identity column, and is not set as part of the input data, then the identity value used is added to the returned data set

```csharp
Task<DataSetRow> InsertDataAsync(string tableName, DataSetRow data)

Task<DataSetRow> InsertDataAsync(string tableName, DataSetRow defaultData, DataSetRow overrideData)
```

Parameters:
* **tableName** - The name of the table to insert into, including schema
* **data** - The data to insert
* **defaultData** - The default data to insert
* **overrideData** - The data to insert, overriding the data provided in defaultData

Returns:
* Returns the data inserted, including the identity value (if applicable)

## `InsertTemplateAsync<T>` (default values)

Inserts data based on the default values defined in the template

Where the table has an identity column, and is not set as part of the input data, then the identity value used is added to the returned data set

```csharp
Task<T> InsertTemplateAsync<T>() where T : ITemplate, new()
```

Types:
* **T** - The type of template to insert

Returns:
* Returns the template object, including the identity value (if applicable)

## `InsertTemplateAsync<T>`

Inserts data based on the supplied template

Where the table has an identity column, and is not set as part of the input data, then the identity value used is added to the returned data set

```csharp
Task<T> InsertTemplateAsync<T>(T template) where T : ITemplate

Task<ITemplate> InsertTemplateAsync(ITemplate template)
```

Types:
* **T** - The type of template to insert

Parameters:
* **template** - The template containing the data to add

Returns:
* Returns the template object, including the identity value (if applicable)



# QueryResult

A set of data (columns and rows) is returned in a `QueryResult` object.  This object has a number of properties and assertion methods which can be used to test the data.  Alternatively, the data itself can be accessed vua the `RawData` property, to return the data as a `DataTable`.


## `TotalRows`

The total number of rows in the data set

```csharp
int TotalRows { get; }
```

Returns:
* Returns the total number of rows in the data set

## `TotalColumns`

The total number of columns in the data set

```csharp
int TotalColumns { get; }
```

Returns:
* Returns the total number of columns in the data set

## `ColumnNames`

The collection of columns in the data set, in the order they appear in the data set

```csharp
ICollection<string> ColumnNames { get; }
```

Returns:
* Returns the collection of columns in the data set, in the order they appear in the data set

## `AssertRowCount`

Asserts the number of rows

```csharp
QueryResult AssertRowCount(int expected)
```

Parameters:
* **expected** - The expected number of rows

Returns:
* Returns the same `QueryResult` object

## `AssertRowCount`

Asserts the number of columns

```csharp
QueryResult AssertColumnCount(int expected)
```

Parameters:
* **expected** - The expected number of columns

Returns:
* Returns the same `QueryResult` object

## `AssertColumnExists`

Asserts that a specific column exists in the data set

```csharp
QueryResult AssertColumnExists(string expectedColumnName)
```

Parameters:
* **expectedColumnName** - The column name (case-sensitive)

Returns:
* Returns the same `QueryResult` object

## `AssertColumnNotExists`

Asserts that a specific column does not exist in the data set

```csharp
QueryResult AssertColumnNotExists(string notExpectedColumnName)
```

Parameters:
* **notExpectedColumnName** - The column name (case-sensitive)

Returns:
* Returns the same `QueryResult` object

## `AssertColumnsExist`

Asserts that a number of columns all exist in the data set

```csharp
QueryResult AssertColumnsExist(params string[] expectedColumnNames)
```

Parameters:
* **expectedColumnNames** - The column names (case-sensitive)

Returns:
* Returns the same `QueryResult` object

## `AssertColumnsNotExist`

Asserts that a number of columns all do not exist in the data set

```csharp
QueryResult AssertColumnsNotExist(params string[] notExpectedColumnNames)
```

Parameters:
* **notExpectedColumnNames** - The column names (case-sensitive)

Returns:
* Returns the same `QueryResult` object

## `AssertRowPositionExists`

Asserts that a row exists at a specific position (zero-based)

```csharp
QueryResult AssertRowPositionExists(int expectedRowPosition)
```

Parameters:
* **expectedRowPosition** - The row position (zero-based)

Returns:
* Returns the same `QueryResult` object

## `AssertValue`

Asserts that a specific value exists for the given row and column.  Also asserts that the row and column exists

```csharp
QueryResult AssertValue(int rowNumber, string columnName, object expectedValue)
```

Parameters:
* **rowNumber** - The row position (zero-based)
* **columnName** - The column name (case-sensitive)
* **expectedValue** - The expected value.  Respects `IComparison` objects

Returns:
* Returns the same `QueryResult` object

## `ValidateRow`

Returns a `RowResult` object, representing the specific row on which further assertions can be made.  Validates that the row number exists in the data set

```csharp
RowResult ValidateRow(int rowNumber)
```

Parameters:
* **rowNumber** - The row number (zero-based)

Returns:
* Returns the `RowResult` for the row

## `AssertRowValues`

Asserts that the row at the given position matches the expected data.  Also asserts that all columns in the expected data exist

```csharp
QueryResult AssertRowValues(int rowNumber, DataSetRow expectedData)
```

Parameters:
* **rowNumber** - The row number (zero-based)
* **expectedData** - The expected data to match.  Respects `IComparison` objects

Returns:
* Returns the same `QueryResult` object

## `AssertRowExists`

Asserts that at least one row matches the expected data.  Also asserts that all columns in the expected data exist

```csharp
QueryResult AssertRowExists(DataSetRow expectedData)
```

Parameters:
* **expectedData** - The expected data to match.  Respects `IComparison` objects

Returns:
* Returns the same `QueryResult` object

## `AssertRowDoesNotExist`

Asserts that no rows match the supplied data.  Also asserts that all columns in the supplied data exist

```csharp
QueryResult AssertRowDoesNotExist(DataSetRow unexpectedData)
```

Parameters:
* **unexpectedData** - The unexpected data.  Respects `IComparison` objects

Returns:
* Returns the same `QueryResult` object


# RowResult

A single row of values is returned in a `RowResult` object, accessed via the `QueryResult.ValidateRow` method.  This object has a number of assertion methods which can be used to test the data in that specific row.

## `AssertValue`

Asserts that a specific value exists for the given column.  Also asserts that the column exists

```csharp
RowResult AssertValue(string columnName, object expectedValue)
```

Parameters:
* **columnName** - The column name (case-sensitive)
* **expectedValue** - The expected value.  Respects `IComparison` objects

Returns:
* Returns the same `RowResult` object

## `AssertValues`

Asserts that the row matches the expected data.  Also asserts that all columns in the expected data exist

```csharp
RowResult AssertValues(DataSetRow expectedData)
```

Parameters:
* **expectedData** - The expected data to match.  Respects `IComparison` objects

Returns:
* Returns the same `RowResult` object

## `ValidateRow`

Returns a `RowResult` object, representing the specific row on which further assertions can be made.  Validates that the row number exists in the data set

```csharp
RowResult ValidateRow(int rowNumber)
```

Parameters:
* **rowNumber** - The row number (zero-based)

Returns:
* Returns the `RowResult` for the row


# ScalarResult

A single value is returned in a `ScalarResult<T>` object, accessed via a scalar method in `TestRunner`.  This object has an assertion method which can be used to test the value.

## `AssertValue`

Asserts that the value matches the expected value

```csharp
ScalarResult<T> AssertValue(object expectedValue)
```

Parameters:
* **expectedValue** - The expected value.  Respects `IComparison` objects

Returns:
* Returns the same `ScalarResult<T>` object