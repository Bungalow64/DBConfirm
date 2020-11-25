using DBConfirm.Core.DataResults;
using DBConfirm.Core.Parameters;
using DBConfirm.Databases.SQLServer.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBConfirm.Databases.SQLServer.Runners.Abstract
{
    /// <summary>
    /// The interface for the execution plan runner, handling all SQL connections for a single database
    /// </summary>
    /// <remarks>When communicating with a database multiple times within a single test, the same execution plan runner instance must be used</remarks>
    public interface ISQLServerExecutionPlanRunner
    {
        /// <summary>
        /// Executes a command, returning a single data table
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>Where the command returns a data set, the first table is returned, otherwise an empty data set is returned</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<QueryResult>> ExecuteCommandAsync(string commandText, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a command, returning a single data table
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>Where the command returns a data set, the first table is returned, otherwise an empty data set is returned</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<QueryResult>> ExecuteCommandAsync(string commandText, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a command, returning all data tables
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>Returns all tables returned from the command</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<IList<QueryResult>>> ExecuteCommandMultipleDataSetAsync(string commandText, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a command, returning all data tables
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>Returns all tables returned from the command</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<IList<QueryResult>>> ExecuteCommandMultipleDataSetAsync(string commandText, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a command, returning no data
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<NoResult>> ExecuteCommandNoResultsAsync(string commandText, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a command, returning no data
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<NoResult>> ExecuteCommandNoResultsAsync(string commandText, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a command, returning a single object
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <typeparam name="T">The type of the object to return</typeparam>
        /// <returns>Returns the object returned from the command</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        Task<ExecutionPlanQueryResult<ScalarResult<T>>> ExecuteCommandScalarAsync<T>(string commandText, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a command, returning a single object
        /// </summary>
        /// <param name="commandText">The command to execute</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <typeparam name="T">The type of the object to return</typeparam>
        /// <returns>Returns the object returned from the command</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        Task<ExecutionPlanQueryResult<ScalarResult<T>>> ExecuteCommandScalarAsync<T>(string commandText, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a stored procedure, returning all data tables
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>Returns all tables returned from the stored procedure</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<IList<QueryResult>>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a stored procedure, returning all data tables
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>Returns all tables returned from the stored procedure</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<IList<QueryResult>>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a stored procedure, returning nothing
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<NoResult>> ExecuteStoredProcedureNonQueryAsync(string procedureName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a stored procedure, returning nothing
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<NoResult>> ExecuteStoredProcedureNonQueryAsync(string procedureName, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a stored procedure, returning a single data table
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <returns>Where the stored procedure returns a data set, the first table is returned, otherwise an empty data set is returned</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<QueryResult>> ExecuteStoredProcedureQueryAsync(string procedureName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a stored procedure, returning a single data table
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <returns>Where the stored procedure returns a data set, the first table is returned, otherwise an empty data set is returned</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<QueryResult>> ExecuteStoredProcedureQueryAsync(string procedureName, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Executes a stored procedure, returning a single object
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used.  The Key is used as the parameter name, and the Value used as the parameter value</param>
        /// <typeparam name="T">The type of the object to return</typeparam>
        /// <returns>Returns the object returned from the stored procedure</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        Task<ExecutionPlanQueryResult<ScalarResult<T>>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes a stored procedure, returning a single object
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure, including schema</param>
        /// <param name="parameters">The parameters to be used</param>
        /// <typeparam name="T">The type of the object to return</typeparam>
        /// <returns>Returns the object returned from the stored procedure</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        Task<ExecutionPlanQueryResult<ScalarResult<T>>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, params SqlQueryParameter[] parameters);
        /// <summary>
        /// Returns all data for a specific view
        /// </summary>
        /// <param name="viewName">The name of the view, including schema</param>
        /// <returns>Returns all columns and values found</returns>
        /// <exception cref="System.Data.Common.DbException"></exception>
        Task<ExecutionPlanQueryResult<QueryResult>> ExecuteViewAsync(string viewName);
    }
}
