using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using DBConfirm.Packages.MySQL.MSTest;
using System;
using DBConfirm.Databases.MySQL.Exceptions;
using MySql.Data.MySqlClient;

namespace Sample.Core.MySQL.MSTest.Tests.Tables
{
    [TestClass]
    public class InvalidTableTests : MSTestBase
    {
        #region Can Query

        [TestMethod]
        public async Task InvalidTable_Query_ReturnError()
        {
            MySqlException exception = await Assert.ThrowsExceptionAsync<MySqlException>(async () => await TestRunner.ExecuteTableAsync("UnknownTable"));

            Assert.AreEqual("Table 'SampleDB.UnknownTable' doesn't exist", exception.Message);
        }

        [TestMethod]
        public async Task InvalidTableWithUnescapedSpecialCharacter_Query_ReturnError()
        {
            MySqlException exception = await Assert.ThrowsExceptionAsync<MySqlException>(async () => await TestRunner.ExecuteTableAsync("UnknownT'able"));

            Assert.AreEqual("Table 'SampleDB.UnknownT'able' doesn't exist", exception.Message);
        }

        [TestMethod]
        public async Task SQLInjectionAttempt_Query_ReturnError()
        {
            MySqlException exception = await Assert.ThrowsExceptionAsync<MySqlException>(async () => await TestRunner.ExecuteTableAsync("Users' -- do something else"));

            Assert.AreEqual("Table 'SampleDB.Users' -- do something else' doesn't exist", exception.Message);
        }

        [TestMethod]
        public async Task TooLongName_Query_ReturnError()
        {
            InvalidOperationException exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await TestRunner.ExecuteTableAsync($"{new string('x', 65)}"));

            Assert.AreEqual("The name of the table or schema cannot be more than 64 characters", exception.Message);
        }

        [TestMethod]
        public async Task MaxLengthButIncorrectName_Query_ReturnError()
        {
            const string _tableName = "aaaaaaaaHYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLI";

            MySqlException exception = await Assert.ThrowsExceptionAsync<MySqlException>(async () => await TestRunner.ExecuteTableAsync($"{_tableName}"));

            Assert.AreEqual("Table 'SampleDB.aaaaaaaaHYBS0J8RHT6YHB9JJIPS2TWZQ2MA9C05I70WYG83LG877Q03X1XBGKLI' doesn't exist", exception.Message);
        }

        #endregion
    }
}
