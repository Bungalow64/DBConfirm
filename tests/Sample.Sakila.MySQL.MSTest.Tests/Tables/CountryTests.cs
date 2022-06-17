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
    public class CountryTests : MSTestBase
    {
        private const string _tableName = "country";

        [TestMethod]
        public async Task CanQuery_EmptyTable()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertColumnsExist("country", "country_id", "last_update");
        }

        [TestMethod]
        public async Task CanInsert_DefaultData()
        {
            await TestRunner.InsertTemplateAsync(new CountryTemplate());

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .ValidateRow(0)
                .AssertValues(new DataSetRow
                {
                    { "country", "SampleCountry" },
                    { "last_update", Comparisons.IsUtcNow() }
                });
        }

        [TestMethod]
        public async Task CanInsert_PopulatedData()
        {
            var country = await TestRunner.InsertTemplateAsync(new CountryTemplate()
                .WithCountry("Country1")
                .WithCountry_id(101)
                .WithLast_update(DateTime.Parse("04-Mar-2021")));

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .ValidateRow(0)
                .AssertValues(new DataSetRow
                {
                    { "country", "Country1" },
                    { "country_id", Convert.ToUInt16(101) },
                    { "last_update", DateTime.Parse("04-Mar-2021") }
                });
        }
    }
}
