using DBConfirm.Core.DataResults;
using DBConfirm.Packages.MySQL.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Core.MySQL.MSTest.Tests.Templates;
using System.Threading.Tasks;

namespace Sample.Core.MySQL.MSTest.Tests.Tables
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

            QueryResult results = await TestRunner.ExecuteTableAsync("NumbersTable");

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
                    .AssertValue("RealColumn", 10d) // Compares real/double values (not single) - see https://dev.mysql.com/doc/refman/8.0/en/numeric-types.html
                    .AssertValue("TinyIntColumn", (byte)10); // Compares tinyint/byte values
        }
    }
}
