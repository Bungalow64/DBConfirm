using Microsoft.Data.SqlClient;
using NUnit.Framework;
using SQLConfirm.Core.Templates.Placeholders;
using SQLConfirm.Core.Exceptions;
using SQLConfirm.Core.Templates;
using System;
using SQLConfirm.Core.Parameters;
using SQLConfirm.Databases.SQLServer.Extensions;

namespace SQLConfirm.Databases.SQLServer.Tests.Extensions
{
    [TestFixture]
    public class SqlQueryParameterExtensionsTests
    {
        [Test]
        public void SqlQueryParameterExtensions_NullArray_ReturnEmptyArray()
        {
            SqlQueryParameter[] array = null;
            SqlParameter[] result = array.ToSqlParameters();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void SqlQueryParameterExtensions_EmptyArray_ReturnEmptyArray()
        {
            SqlQueryParameter[] array = new SqlQueryParameter[0];
            SqlParameter[] result = array.ToSqlParameters();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void SqlQueryParameterExtensions_ArrayWithItems_ReturnArrayWithItems()
        {
            SqlQueryParameter[] array = new SqlQueryParameter[]
            {
                new SqlQueryParameter("ColumnA", 123)
            };
            SqlParameter[] result = array.ToSqlParameters();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("@ColumnA", result[0].ParameterName);
            Assert.AreEqual(123, result[0].Value);
        }

        [Test]
        public void SqlQueryParameterExtensions_ArrayWithRequiredPlaceholder_ThrowError()
        {
            SqlQueryParameter[] array = new SqlQueryParameter[]
            {
                new SqlQueryParameter("ColumnA", 123),
                new SqlQueryParameter("ColumnB", Placeholders.IsRequired())
            };
            RequiredPlaceholderIsNullException exception = Assert.Throws<RequiredPlaceholderIsNullException>(() => array.ToSqlParameters());
            Assert.AreEqual("ColumnB", exception.ColumnName);
            Assert.AreEqual("The value for ColumnB is required but has not been set", exception.Message);
        }

        [Test]
        public void SqlQueryParameterExtensions_ArrayWithMultipleRequiredPlaceholder_ThrowFirstError()
        {
            SqlQueryParameter[] array = new SqlQueryParameter[]
            {
                new SqlQueryParameter("ColumnA", Placeholders.IsRequired()),
                new SqlQueryParameter("ColumnB", Placeholders.IsRequired())
            };
            RequiredPlaceholderIsNullException exception = Assert.Throws<RequiredPlaceholderIsNullException>(() => array.ToSqlParameters());
            Assert.AreEqual("ColumnA", exception.ColumnName);
            Assert.AreEqual("The value for ColumnA is required but has not been set", exception.Message);
        }

        [Test]
        public void SqlQueryParameterExtensions_ArrayWithResolver_CallResolverAndReturnResult()
        {
            Func<int> resolveAction = () => 123;

            SqlQueryParameter[] array = new SqlQueryParameter[]
            {
                new SqlQueryParameter("ColumnA", new Resolver<int>(resolveAction))
            };
            SqlParameter[] result = array.ToSqlParameters();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("@ColumnA", result[0].ParameterName);
            Assert.AreEqual(123, result[0].Value);
        }

        [Test]
        public void SqlQueryParameterExtensions_ArrayWithNull_ReturnDBNull()
        {
            SqlQueryParameter[] array = new SqlQueryParameter[]
            {
                new SqlQueryParameter("ColumnA", null)
            };
            SqlParameter[] result = array.ToSqlParameters();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("@ColumnA", result[0].ParameterName);
            Assert.AreEqual(DBNull.Value, result[0].Value);
        }
    }
}
