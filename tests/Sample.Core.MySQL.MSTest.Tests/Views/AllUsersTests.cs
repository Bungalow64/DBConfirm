using DBConfirm.Core.Data;
using DBConfirm.Core.DataResults;
using DBConfirm.Core.Parameters;
using DBConfirm.Packages.MySQL.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Core.MySQL.MSTest.Tests.Templates;
using Sample.Core.MySQL.MSTest.Tests.Templates.Complex;
using System;
using System.Threading.Tasks;

namespace Sample.Core.MySQL.MSTest.Tests.Views
{
    [TestClass]
    public class AllUsersTests : MSTestBase
    {
        [TestMethod]
        public async Task AllUsers_NoData_NothingReturned()
        {
            QueryResult results = await TestRunner.ExecuteViewAsync("AllUsers");

            results
                .AssertRowCount(0);

            Assert.AreEqual(0, await TestRunner.CountRowsInViewAsync("AllUsers"));
        }

        [TestMethod]
        public async Task AllUsers_OneRow_OneUserReturned()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("AddUser",
                new SqlQueryParameter("p_FirstName", "Jamie"),
                new SqlQueryParameter("p_LastName", "Burns"),
                new SqlQueryParameter("p_EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("p_StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("p_NumberOfHats", 14),
                new SqlQueryParameter("p_Cost", 15.87));

            QueryResult results = await TestRunner.ExecuteViewAsync("AllUsers");

            results
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" }
                });

            Assert.AreEqual(1, await TestRunner.CountRowsInViewAsync("AllUsers"));
        }

        [TestMethod]
        public async Task AllUsers_TwoRows_TwoUsersReturned()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("AddUser",
                new SqlQueryParameter("p_FirstName", "Jamie"),
                new SqlQueryParameter("p_LastName", "Burns"),
                new SqlQueryParameter("p_EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlQueryParameter("p_StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("p_NumberOfHats", 14),
                new SqlQueryParameter("p_Cost", 15.87));

            await TestRunner.ExecuteStoredProcedureNonQueryAsync("AddUser",
                new SqlQueryParameter("p_FirstName", "Stuart"),
                new SqlQueryParameter("p_LastName", "Burns"),
                new SqlQueryParameter("p_EmailAddress", "stuart@bungalow64.co.uk"),
                new SqlQueryParameter("p_StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlQueryParameter("p_NumberOfHats", 14),
                new SqlQueryParameter("p_Cost", 15.87));

            QueryResult results = await TestRunner.ExecuteViewAsync("AllUsers");

            results
                .AssertRowCount(2)
                .AssertRowExists(new DataSetRow
                {
                    { "FirstName", "Jamie" }
                })
                .AssertRowExists(new DataSetRow
                {
                    { "FirstName", "Stuart" }
                });
        }

        [TestMethod]
        public async Task AllUsers_UseComplexData_ReuseTemplate()
        {
            UserTemplate user = new UserTemplate
            {
                { "FirstName", "Jamie" }
            };

            UserWithAddressTemplate userWithAddress = new UserWithAddressTemplate
            {
                User = user
            };
            await TestRunner.InsertTemplateAsync(userWithAddress);

            await TestRunner.InsertTemplateAsync(user);

            QueryResult results = await TestRunner.ExecuteViewAsync("AllUsers");

            results
                .AssertRowCount(1);
        }

        [TestMethod]
        public async Task AllUsers_UseFluent()
        {
            UserTemplate user = new UserTemplate()
                .WithId(1001)
                .WithFirstName("Jamie");

            UserWithAddressTemplate userWithAddress = new UserWithAddressTemplate
            {
                User = user
            };
            await TestRunner.InsertTemplateAsync(userWithAddress);
            await TestRunner.InsertTemplateAsync(user);

            QueryResult results = await TestRunner.ExecuteViewAsync("AllUsers");

            results
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "Id", 1001 },
                    { "FirstName", "Jamie" }
                });
        }

        [TestMethod]
        public async Task AllUsers_UseFluentReversed()
        {
            UserTemplate user = new UserTemplate()
                .WithId(1002)
                .WithFirstName("Jamie");

            UserWithAddressTemplate userWithAddress = new UserWithAddressTemplate
            {
                User = user
            };
            await TestRunner.InsertTemplateAsync(user);
            await TestRunner.InsertTemplateAsync(userWithAddress);

            QueryResult results = await TestRunner.ExecuteViewAsync("AllUsers");

            results
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "Id", 1002 },
                    { "FirstName", "Jamie" }
                });
        }

        [TestMethod]
        public async Task AllUsers_DefaultComplex()
        {
            UserWithAddressTemplate template = await TestRunner.InsertTemplateAsync<UserWithAddressTemplate>();

            QueryResult results = await TestRunner.ExecuteViewAsync("AllUsers");

            results
                .AssertRowCount(1)
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", template.User.DefaultData["FirstName"] }
                });
        }
    }
}
