using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Packages.MySQL.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Sakila.MySQL.MSTest.Tests.Templates;
using System;
using System.Threading.Tasks;

namespace Sample.Sakila.MySQL.MSTest.Tests.Tables
{
    [TestClass]
    public class CategoryTests : MSTestBase
    {
        private const string _tableName = "category";

        [TestMethod]
        public async Task CanQuery_EmptyTable()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertColumnsExist("category_id", "last_update", "name");
        }

        [TestMethod]
        public async Task CanInsert_DefaultData()
        {
            await TestRunner.InsertTemplateAsync(new CategoryTemplate());

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .ValidateRow(0)
                .AssertValues(new DataSetRow
                {
                    { "name", "SampleName" },
                    { "last_update", Comparisons.IsUtcNow() }
                });
        }

        [TestMethod]
        public async Task CanInsert_PopulatedData()
        {
            await TestRunner.InsertTemplateAsync(new CategoryTemplate()
                .WithName("Jamie")
                .WithLast_update(DateTime.Parse("04-Mar-2021"))
                .WithCategory_id(96));

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .ValidateRow(0)
                .AssertValues(new DataSetRow
                {
                    { "name", "Jamie" },
                    { "last_update", DateTime.Parse("04-Mar-2021") },
                    { "category_id", (byte)96 }
                });
        }
    }
}
