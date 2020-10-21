using SQLConfirm.Core.DataResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLConfirm.Core.TestFrameworks.Abstract;
using SQLConfirm.Core.Templates.Abstract;
using SQLConfirm.Core.Data;
using SQLConfirm.Core.Parameters;

namespace SQLConfirm.Core.Runners.Abstract
{
    /// <summary>
    /// The interface for the test runner, handling all SQL connections for a single database
    /// </summary>
    /// <remarks>When communicating with a database multiple times within a single test, the same test runner instance must be used</remarks>
    public interface ITestRunner : IDisposable
    {
        /// <summary>
        /// Sets up the test runner, based on the specified test framework.  This also opens the connection to the database, and starts the transaction to be used throughout the test
        /// </summary>
        /// <param name="testFramework">The test framework to be used for assertions</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task InitialiseAsync(ITestFramework testFramework);
        /// <summary>
        /// Returns the total number of rows in the table
        /// </summary>
        /// <param name="tableName">The name of the table, including schema</param>
        /// <returns>Returns the total number of rows</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<int> CountRowsInTableAsync(string tableName);
        /// <summary>
        /// Returns the total number of rows in the view
        /// </summary>
        /// <param name="viewName">The name of the view, including schema</param>
        /// <returns>Returns the total number of rows</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<int> CountRowsInViewAsync(string viewName);
        /// <summary>
        /// Executes a command, returning a single data table
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>Where the command returns a data set, the first table is returned, otherwise an empty data set is returned</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<QueryResult> ExecuteCommandAsync(string commandText, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a command, returning a single data table
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>Where the command returns a data set, the first table is returned, otherwise an empty data set is returned</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<QueryResult> ExecuteCommandAsync(string commandText, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a command, returning all data tables
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>Returns all tables returned from the command</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a command, returning all data tables
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>Returns all tables returned from the command</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a command, returning no data
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task ExecuteCommandNoResultsAsync(string commandText, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a command, returning no data
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task ExecuteCommandNoResultsAsync(string commandText, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a command, returning a single object
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <typeparam name="T">The type of the object to return</typeparam>
        /// <returns>Returns the object returned from the command</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a command, returning a single object
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <typeparam name="T">The type of the object to return</typeparam>
        /// <returns>Returns the object returned from the command</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a stored procedure, returning all data tables
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>Returns all tables returned from the stored procedure</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a stored procedure, returning all data tables
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>Returns all tables returned from the stored procedure</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a stored procedure, returning nothing
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task ExecuteStoredProcedureNonQueryAsync(string procedureName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a stored procedure, returning nothing
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task ExecuteStoredProcedureNonQueryAsync(string procedureName, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a stored procedure, returning a single data table
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>Where the stored procedure returns a data set, the first table is returned, otherwise an empty data set is returned</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a stored procedure, returning a single data table
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>Where the stored procedure returns a data set, the first table is returned, otherwise an empty data set is returned</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a stored procedure, returning a single object
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <typeparam name="T">The type of the object to return</typeparam>
        /// <returns>Returns the object returned from the stored procedure</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a stored procedure, returning a single object
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <typeparam name="T">The type of the object to return</typeparam>
        /// <returns>Returns the object returned from the stored procedure</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Returns all data for a specific table
        /// </summary>
        /// <param name="tableName">The name of the table, including schema</param>
        /// <returns>Returns all columns and values found</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<QueryResult> ExecuteTableAsync(string tableName);
        /// <summary>
        /// Returns all data for a specific view
        /// </summary>
        /// <param name="viewName">The name of the view, including schema</param>
        /// <returns>Returns all columns and values found</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<QueryResult> ExecuteViewAsync(string viewName);

        /// <summary>
        /// Inserts data into a table
        /// </summary>
        /// <remarks>Where the table has an identity column, and is not set as part of the input data, then the identity value used is added to the returned data set</remarks>
        /// <param name="tableName">The name of the table to insert into, including schema</param>
        /// <param name="data">The data to insert</param>
        /// <returns>Returns the data inserted, including the identity value (if applicable)</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<DataSetRow> InsertDataAsync(string tableName, DataSetRow data);
        /// <summary>
        /// Inserts data into a table
        /// </summary>
        /// <remarks>Where the table has an identity column, and is not set as part of the input data, then the identity value used is added to the returned data set.  Data from both <paramref name="defaultData"/> and <paramref name="overrideData"/> is used, however where the same columns are specified in both data sets, then the value in <paramref name="overrideData"/> is used</remarks>
        /// <param name="tableName">The name of the table to insert into, including schema</param>
        /// <param name="defaultData">The default data to insert</param>
        /// <param name="overrideData">The data to insert, overriding the data provided in <paramref name="defaultData"/></param>
        /// <returns>Returns the data inserted, including the identity value (if applicable)</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<DataSetRow> InsertDataAsync(string tableName, DataSetRow defaultData, DataSetRow overrideData);

        /// <summary>
        /// Inserts data based on the default values defined in the template
        /// </summary>
        /// <remarks>Where the table has an identity column, and is not set as part of the input data, then the identity value used is added to the returned data set</remarks>
        /// <typeparam name="T">The type of template to insert</typeparam>
        /// <returns>Returns the template object, including the identity value (if applicable)</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<T> InsertTemplateAsync<T>() where T : ITemplate, new();
        /// <summary>
        /// Inserts data based on the supplied template
        /// </summary>
        /// <remarks>Where the table has an identity column, and is not set as part of the input data, then the identity value used is added to the returned data set</remarks>
        /// <typeparam name="T">The type of template to insert</typeparam>
        /// <param name="template">The template containing the data to add</param>
        /// <returns>Returns the template object, including the identity value (if applicable)</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<T> InsertTemplateAsync<T>(T template) where T : ITemplate;
        /// <summary>
        /// Inserts data based on the supplied template
        /// </summary>
        /// <remarks>Where the table has an identity column, and is not set as part of the input data, then the identity value used is added to the returned data set</remarks>
        /// <param name="template">The template containing the data to add</param>
        /// <returns>Returns the template object, including the identity value (if applicable)</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ITemplate> InsertTemplateAsync(ITemplate template);
    }
}
