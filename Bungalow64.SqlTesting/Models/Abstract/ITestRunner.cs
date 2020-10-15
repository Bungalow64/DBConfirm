using Microsoft.Data.SqlClient;
using Models.Templates.Asbtract;
using Models.DataResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.TestFrameworks.Abstract;

namespace Models.Abstract
{
    public interface ITestRunner : IDisposable
    {
        Task<int> CountRowsInTableAsync(string tableName);
        Task<int> CountRowsInViewAsync(string viewName);
        Task<QueryResult> ExecuteCommandAsync(string commandText, IDictionary<string, object> parameters);
        Task<QueryResult> ExecuteCommandAsync(string commandText, params SqlParameter[] parameters);
        Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, IDictionary<string, object> parameters);
        Task<IList<QueryResult>> ExecuteCommandMultipleDataSetAsync(string commandText, params SqlParameter[] parameters);
        Task ExecuteCommandNoResultsAsync(string commandText, IDictionary<string, object> parameters);
        Task ExecuteCommandNoResultsAsync(string commandText, params SqlParameter[] parameters);
        Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, IDictionary<string, object> parameters);
        Task<ScalarResult<T>> ExecuteCommandScalarAsync<T>(string commandText, params SqlParameter[] parameters);
        Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, IDictionary<string, object> parameters);
        Task<IList<QueryResult>> ExecuteStoredProcedureMultipleDataSetAsync(string procedureName, params SqlParameter[] parameters);
        Task ExecuteStoredProcedureNonQueryAsync(string procedureName, params SqlParameter[] parameters);
        Task<QueryResult> ExecuteStoredProcedureQueryAsync(string procedureName, params SqlParameter[] parameters);
        Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, IDictionary<string, object> parameters);
        Task<ScalarResult<T>> ExecuteStoredProcedureScalarAsync<T>(string procedureName, params SqlParameter[] parameters);
        Task<QueryResult> ExecuteTableAsync(string tableName);
        Task<QueryResult> ExecuteViewAsync(string viewName);
        Task InitialiseAsync(ITestFramework testFramework);
        Task<T> InsertAsync<T>() where T : ITemplate, new();
        Task<T> InsertAsync<T>(T template) where T : ITemplate;
        Task<T> InsertComplexAsync<T>(T complexTemplate) where T : IComplexTemplate;
        Task<DataSetRow> InsertDataAsync(string tableName, DataSetRow data);
    }
}
