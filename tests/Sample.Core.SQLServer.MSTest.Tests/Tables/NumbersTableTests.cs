using DBConfirm.Core.DataResults;
using DBConfirm.Packages.SQLServer.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Core.SQLServer.MSTest.Tests.Templates;
using System.Threading.Tasks;

namespace Sample.Core.SQLServer.MSTest.Tests.Tables
{
    [TestClass]
    public class NumbersTableTests : MSTestBase
    {
        [TestMethod]
        public async Task NumbersTable_AssertAllNumbers()
        {
            await TestRunner.InsertTemplateAsync(new NumbersTableTemplate
            {
                ["IntColumn"] = 10,
                ["SmallIntColumn"] = 10,
                ["BigIntColumn"] = 10,
                ["DecimalColumn"] = 10,
                ["MoneyColumn"] = 10,
                ["SmallMoneyColumn"] = 10,
                ["NumericColumn"] = 10,
                ["FloatColumn"] = 10,
                ["RealColumn"] = 10,
                ["TinyIntColumn"] = 10
            });

            QueryResult results = await TestRunner.ExecuteTableAsync("dbo.NumbersTable");

            results
                .ValidateRow(0)
                    .AssertValue("IntColumn", 10) // Compares int/Int32 values
                    .AssertValue("SmallIntColumn", (short)10) // Compares smallint/short/Int16 values
                    .AssertValue("BigIntColumn", 10L) // Compares bigint/long/Int64 values
                    .AssertValue("DecimalColumn", 10m) // Compares decimal values
                    .AssertValue("MoneyColumn", 10m) // Compares money values
                    .AssertValue("SmallMoneyColumn", 10m) // Compares smallmoney values
                    .AssertValue("NumericColumn", 10m) // Compares numeric values
                    .AssertValue("FloatColumn", 10d) // Compares float/double values
                    .AssertValue("RealColumn", 10f) // Compares real/single values
                    .AssertValue("TinyIntColumn", (byte)10); // Compares tinyint/byte values
        }
    }
}
