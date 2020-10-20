using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Core.MSTest.Tests.Templates;
using Sample.Core.MSTest.Tests.Templates.Complex;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DataResults;
using Frameworks.MSTest2;
using Models.Data;

namespace Sample.Core.MSTest.Tests.StoredProcedures
{
    [TestClass]
    public class GetUserTests : TestBase
    {
        [TestMethod]
        public async Task GetUser_NoData_ReturnNoRows()
        {
            QueryResult result = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.GetUser", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"));

            result
                .AssertRowCount(0);
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ReturnNames()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            QueryResult result = await TestRunner.ExecuteStoredProcedureQueryAsync("dbo.GetUser", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"));

            result
                .AssertRowCount(1)
                .AssertColumnsExist("FirstName", "LastName")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" }
                });
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ReturnNamesAndCount()
        {
            await TestRunner.ExecuteStoredProcedureNonQueryAsync("dbo.AddUser",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            IList<QueryResult> result = await TestRunner.ExecuteStoredProcedureMultipleDataSetAsync("dbo.GetUser", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"));

            result[0]
                .AssertRowCount(1)
                .AssertColumnsExist("FirstName", "LastName")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" }
                });

            result[1]
                .AssertRowCount(1)
                .AssertColumnsExist("TotalUsers")
                .AssertValue(0, "TotalUsers", 1);
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ReturnNamesAndCount_ByCommand()
        {
            await TestRunner.ExecuteCommandNoResultsAsync("EXEC dbo.AddUser @FirstName, @LastName, @EmailAddress, @StartDate, @NumberOfHats, @Cost",
                new SqlParameter("FirstName", "Jamie"),
                new SqlParameter("LastName", "Burns"),
                new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"),
                new SqlParameter("StartDate", DateTime.Parse("01-Mar-2020")),
                new SqlParameter("NumberOfHats", 14),
                new SqlParameter("Cost", 15.87));

            IList<QueryResult> result = await TestRunner.ExecuteCommandMultipleDataSetAsync("EXEC dbo.GetUser @EmailAddress", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"));

            result[0]
                .AssertRowCount(1)
                .AssertColumnsExist("FirstName", "LastName")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" }
                });

            result[1]
                .AssertRowCount(1)
                .AssertColumnsExist("TotalUsers")
                .AssertValue(0, "TotalUsers", 1);
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ReturnPostcode()
        {
            UserTemplate user = new UserTemplate();
            UserAddressTemplate userA = new UserAddressTemplate(user);
            await TestRunner.InsertTemplateAsync(user);
            await TestRunner.InsertTemplateAsync(userA);

            IList<QueryResult> result = await TestRunner.ExecuteStoredProcedureMultipleDataSetAsync("dbo.GetUser", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"));

            result[0]
                .AssertRowCount(1)
                .AssertColumnsExist("FirstName", "LastName", "Postcode")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 3UB" }
                });

            result[1]
                .AssertRowCount(1)
                .AssertColumnsExist("TotalUsers")
                .AssertValue(0, "TotalUsers", 1);
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ReturnMultiplePostcodes()
        {
            UserTemplate user = await TestRunner.InsertTemplateAsync<UserTemplate>();
            await TestRunner.InsertTemplateAsync(new UserAddressTemplate
            {
                { "UserId", user.Identity },
                { "Postcode", "HD6 3UB" }
            });
            await TestRunner.InsertTemplateAsync(new UserAddressTemplate
            {
                { "UserId", user.Identity },
                { "Postcode", "HD6 4UB" }
            });

            IList<QueryResult> result = await TestRunner.ExecuteStoredProcedureMultipleDataSetAsync("dbo.GetUser", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"));

            result[0]
                .AssertRowCount(2)
                .AssertColumnsExist("FirstName", "LastName", "Postcode")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 3UB" }
                })
                .AssertRowValues(1, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 4UB" }
                });

            result[1]
                .AssertRowCount(1)
                .AssertColumnsExist("TotalUsers")
                .AssertValue(0, "TotalUsers", 1);
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ReturnUsers()
        {
            UserTemplate user1 = await TestRunner.InsertTemplateAsync(new UserTemplate
            {
                { "FirstName", "user1" },
                { "EmailAddress", "user1@b64.co.uk" }
            });
            UserTemplate user2 = await TestRunner.InsertTemplateAsync(new UserTemplate
            {
                { "FirstName", "user2" },
                { "EmailAddress", "user1@b64.co.uk" }
            });
            await TestRunner.InsertTemplateAsync(new UserAddressTemplate
            {
                { "UserId", user1.Identity },
                { "Postcode", "HD6 3UB" }
            });
            await TestRunner.InsertTemplateAsync(new UserAddressTemplate
            {
                { "UserId", user2.Identity },
                { "Postcode", "HD6 4UB" }
            });

            IList<QueryResult> result = await TestRunner.ExecuteStoredProcedureMultipleDataSetAsync("dbo.GetUser", new SqlParameter("EmailAddress", "user1@b64.co.uk"));

            result[0]
                .AssertRowCount(2)
                .AssertColumnsExist("FirstName", "LastName", "Postcode")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "user1" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 3UB" }
                })
                .AssertRowValues(1, new DataSetRow
                {
                    { "FirstName", "user2" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 4UB" }
                });

            result[1]
                .AssertRowCount(1)
                .AssertColumnsExist("TotalUsers")
                .AssertValue(0, "TotalUsers", 2);
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ViaComplex_ReturnUsers()
        {
            await TestRunner.InsertTemplateAsync(new UserWithAddressTemplate
            {
                User = new UserTemplate
                {
                    { "FirstName", "user1" },
                    { "EmailAddress", "user1@b64.co.uk" }
                },
                UserAddress = new UserAddressTemplate
                {
                    { "Postcode", "HD6 3UB" }
                }
            });

            await TestRunner.InsertTemplateAsync(new UserWithAddressTemplate
            {
                User = new UserTemplate
                {
                    { "FirstName", "user2" },
                    { "EmailAddress", "user1@b64.co.uk" }
                },
                UserAddress = new UserAddressTemplate
                {
                    { "Postcode", "HD6 4UB" }
                }
            });

            IList<QueryResult> result = await TestRunner.ExecuteStoredProcedureMultipleDataSetAsync("dbo.GetUser", new SqlParameter("EmailAddress", "user1@b64.co.uk"));

            result[0]
                .AssertRowCount(2)
                .AssertColumnsExist("FirstName", "LastName", "Postcode")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "user1" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 3UB" }
                })
                .AssertRowValues(1, new DataSetRow
                {
                    { "FirstName", "user2" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 4UB" }
                });

            result[1]
                .AssertRowCount(1)
                .AssertColumnsExist("TotalUsers")
                .AssertValue(0, "TotalUsers", 2);
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ViaComplexSameAddress_ReturnUsers()
        {
            await TestRunner.InsertTemplateAsync(new UserWithAddressTemplate
            {
                User = new UserTemplate
                {
                    { "FirstName", "user1" },
                    { "EmailAddress", "user1@b64.co.uk" }
                }
            });

            await TestRunner.InsertTemplateAsync(new UserWithAddressTemplate
            {
                User = new UserTemplate
                {
                    { "FirstName", "user2" },
                    { "EmailAddress", "user1@b64.co.uk" }
                }
            });

            IList<QueryResult> result = await TestRunner.ExecuteStoredProcedureMultipleDataSetAsync("dbo.GetUser", new SqlParameter("EmailAddress", "user1@b64.co.uk"));

            result[0]
                .AssertRowCount(2)
                .AssertColumnsExist("FirstName", "LastName", "Postcode")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "user1" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 3UB" }
                })
                .AssertRowValues(1, new DataSetRow
                {
                    { "FirstName", "user2" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 3UB" }
                });

            result[1]
                .AssertRowCount(1)
                .AssertColumnsExist("TotalUsers")
                .AssertValue(0, "TotalUsers", 2);
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ViaComplexAllDefault_ReturnUsers()
        {
            await TestRunner.InsertTemplateAsync(new UserWithAddressTemplate());
            await TestRunner.InsertTemplateAsync(new UserWithAddressTemplate());

            IList<QueryResult> result = await TestRunner.ExecuteStoredProcedureMultipleDataSetAsync("dbo.GetUser", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"));

            result[0]
                .AssertRowCount(2)
                .AssertColumnsExist("FirstName", "LastName", "Postcode")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 3UB" }
                })
                .AssertRowValues(1, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 3UB" }
                });

            result[1]
                .AssertRowCount(1)
                .AssertColumnsExist("TotalUsers")
                .AssertValue(0, "TotalUsers", 2);
        }

        [TestMethod]
        public async Task GetUser_MatchEmailAddress_ViaComplexOneUserTwoAddress_ReturnUser()
        {
            await TestRunner.InsertTemplateAsync(new UserWithTwoAddressesTemplate());

            IList<QueryResult> result = await TestRunner.ExecuteStoredProcedureMultipleDataSetAsync("dbo.GetUser", new SqlParameter("EmailAddress", "jamie@bungalow64.co.uk"));

            result[0]
                .AssertRowCount(2)
                .AssertColumnsExist("FirstName", "LastName", "Postcode")
                .AssertRowValues(0, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 3UB" }
                })
                .AssertRowValues(1, new DataSetRow
                {
                    { "FirstName", "Jamie" },
                    { "LastName", "Burns" },
                    { "Postcode", "HD6 3UB" }
                });

            result[1]
                .AssertRowCount(1)
                .AssertColumnsExist("TotalUsers")
                .AssertValue(0, "TotalUsers", 1);
        }
    }
}
