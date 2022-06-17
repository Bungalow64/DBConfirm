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
    public class CityTests : MSTestBase
    {
        private const string _tableName = "city";

        [TestMethod]
        public async Task CanQuery_EmptyTable()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertColumnsExist("city", "city_id", "country_id", "last_update");
        }

        [TestMethod]
        public async Task CanInsert_DefaultData()
        {
            var country = await TestRunner.InsertTemplateAsync(new CountryTemplate());
            await TestRunner.InsertTemplateAsync(new CityTemplate().WithCountry_id(country.Identity));

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .ValidateRow(0)
                .AssertValues(new DataSetRow
                {
                    { "city", "SampleCity" },
                    { "country_id", Convert.ToUInt16(country.Identity) },
                    { "last_update", Comparisons.IsUtcNow() }
                });
        }

        [TestMethod]
        public async Task CanInsert_PopulatedData()
        {
            var country = await TestRunner.InsertTemplateAsync(new CountryTemplate());
            await TestRunner.InsertTemplateAsync(new CityTemplate()
                .WithCity("City1")
                .WithCity_id(101)
                .WithCountry_id(country.Identity)
                .WithLast_update(DateTime.Parse("04-Mar-2021")));

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .ValidateRow(0)
                .AssertValues(new DataSetRow
                {
                    { "city", "City1" },
                    { "city_id", Convert.ToUInt16(101) },
                    { "country_id", Convert.ToUInt16(country.Identity) },
                    { "last_update", DateTime.Parse("04-Mar-2021") }
                });
        }
    }
}
