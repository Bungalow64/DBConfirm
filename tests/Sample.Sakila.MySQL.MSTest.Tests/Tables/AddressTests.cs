using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Packages.MySQL.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.Types;
using Sample.Sakila.MySQL.MSTest.Tests.Templates;
using System;
using System.Threading.Tasks;

namespace Sample.Sakila.MySQL.MSTest.Tests.Tables
{
    [TestClass]
    public class AddressTests : MSTestBase
    {
        private const string _tableName = "address";

        [TestMethod]
        public async Task CanQuery_EmptyTable()
        {
            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertColumnsExist("address", "address2", "address_id", "city_id", "district", "last_update", "location", "phone", "postal_code");
        }

        [TestMethod]
        public async Task CanInsert_DefaultData()
        {
            var country = await TestRunner.InsertTemplateAsync(new CountryTemplate());

            var city = await TestRunner.InsertTemplateAsync(new CityTemplate()
                .WithCountry_id(country.Identity));

            await TestRunner.InsertTemplateAsync(new AddressTemplate()
                .WithCity_id(city.Identity));

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .ValidateRow(0)
                .AssertValues(new DataSetRow
                {
                    { "location", new MySqlGeometry(1, 1).Value },
                    { "address", "SampleAddress" },
                    { "address2", null },
                    { "city_id", Convert.ToUInt16(city.Identity) },
                    { "district", "SampleDistrict" },
                    { "last_update", Comparisons.IsUtcNow() },
                    { "phone", "SamplePhone" },
                    { "postal_code", null }
                });
        }

        [TestMethod]
        public async Task CanInsert_PopulatedData()
        {
            var country = await TestRunner.InsertTemplateAsync(new CountryTemplate());

            var city = await TestRunner.InsertTemplateAsync(new CityTemplate()
                .WithCountry_id(country.Identity));

            await TestRunner.InsertTemplateAsync(new AddressTemplate()
                .WithCity_id(city.Identity)
                .WithAddress_id(1001)
                .WithAddress("A1")
                .WithAddress2("A2")
                .WithDistrict("D1")
                .WithLast_update(DateTime.Parse("04-Mar-2021"))
                .WithLocation(new MySqlGeometry(4, 4))
                .WithPhone("P1")
                .WithPostal_code("P2"));

            QueryResult results = await TestRunner.ExecuteTableAsync(_tableName);

            results
                .AssertRowCount(1)
                .ValidateRow(0)
                .AssertValues(new DataSetRow
                {
                    { "location", new MySqlGeometry(4, 4).Value },
                    { "address", "A1" },
                    { "address2", "A2" },
                    { "city_id", Convert.ToUInt16(city.Identity) },
                    { "district", "D1" },
                    { "last_update", DateTime.Parse("04-Mar-2021") },
                    { "phone", "P1" },
                    { "postal_code", "P2" },
                    { "address_id", (UInt16)1001 }
                });
        }
    }
}
