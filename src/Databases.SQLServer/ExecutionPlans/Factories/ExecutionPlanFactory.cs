using DBConfirm.Core.DataResults.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Databases.SQLServer.ExecutionPlans.Factories.Abstract;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace DBConfirm.Databases.SQLServer.ExecutionPlans.Factories
{
    /// <inheritdoc/>
    public class ExecutionPlanFactory : IExecutionPlanFactory
    {
        #region Constants

        private const string _executionPlanXmlPattern = @"^<ShowPlanXML xmlns=""http://schemas.microsoft.com/sqlserver/2004/07/showplan"" Version=""\d*\.?\d*"" Build=""(\d*).*"">";

        #endregion

        /// <inheritdoc/>
        public IExecutionPlan Build(ITestFramework testFramework, DataSet dataSet)
        {
            if (dataSet == null || dataSet.Tables.Count == 0)
            {
                return null;
            }

            Regex match = new Regex(_executionPlanXmlPattern);

            var executionPlans = dataSet.Tables
                .OfType<DataTable>()
                .Where(p => p.Rows.Count == 1 && p.Columns.Count == 1)
                .Select(p => p.Rows[0][0].ToString())
                .Select(p => new { Version = match.Match(p), FullXml = p })
                .Where(p => p.Version.Success && p.Version.Groups.Count >= 2)
                .ToArray();

            if (executionPlans.Length == 0)
            {
                return null;
            }

            string version = executionPlans[0].Version.Groups[1].Value;

            switch (version)
            {
                case "14":
                    return executionPlans.Length == 1 
                        ? new SQLServer2017.ExecutionPlan(testFramework, executionPlans[0].FullXml) 
                        : new SQLServer2017.ExecutionPlanSet(testFramework, executionPlans.Select(p => p.FullXml)) as IExecutionPlan;
                case "15":
                    return executionPlans.Length == 1 
                        ? new SQLServer2019.ExecutionPlan(testFramework, executionPlans[0].FullXml) 
                        : new SQLServer2019.ExecutionPlanSet(testFramework, executionPlans.Select(p => p.FullXml)) as IExecutionPlan;

            }

            return null;
        }
    }
}
