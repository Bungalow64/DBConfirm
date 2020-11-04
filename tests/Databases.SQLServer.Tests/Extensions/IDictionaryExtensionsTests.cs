using Microsoft.Data.SqlClient;
using NUnit.Framework;
using System.Collections.Generic;
using DBConfirm.Databases.SQLServer.Extensions;
using DBConfirm.Core.Templates.Placeholders;
using DBConfirm.Core.Exceptions;
using DBConfirm.Core.Templates;
using System;

namespace DBConfirm.Databases.SQLServer.Tests.Extensions
{
    [TestFixture]
    public class IDictionaryExtensionsTests
    {
        [Test]
        public void IDictionaryExtensions_NullDictionary_ReturnEmptyArray()
        {
            IDictionary<string, object> dictionary = null;
            SqlParameter[] result = dictionary.ToSqlParameters();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void IDictionaryExtensions_EmptyDictionary_ReturnEmptyArray()
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            SqlParameter[] result = dictionary.ToSqlParameters();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void IDictionaryExtensions_DictionaryWithItems_ReturnArrayWithItems()
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "ColumnA", 123 }
            };
            SqlParameter[] result = dictionary.ToSqlParameters();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("@ColumnA", result[0].ParameterName);
            Assert.AreEqual(123, result[0].Value);
        }

        [Test]
        public void IDictionaryExtensions_DictionaryWithRequiredPlaceholder_ThrowError()
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "ColumnA", 123 },
                { "ColumnB", Placeholders.IsRequired() }
            };
            RequiredPlaceholderIsNullException exception = Assert.Throws<RequiredPlaceholderIsNullException>(() => dictionary.ToSqlParameters());
            Assert.AreEqual("ColumnB", exception.ColumnName);
            Assert.AreEqual(null, exception.TableName);
            Assert.AreEqual("The value for ColumnB is required but has not been set", exception.Message);
        }

        [Test]
        public void IDictionaryExtensions_DictionaryWithRequiredPlaceholder_WithTableName_ThrowError()
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "ColumnA", 123 },
                { "ColumnB", Placeholders.IsRequired() }
            };
            RequiredPlaceholderIsNullException exception = Assert.Throws<RequiredPlaceholderIsNullException>(() => dictionary.ToSqlParameters("TableA"));
            Assert.AreEqual("ColumnB", exception.ColumnName);
            Assert.AreEqual("TableA", exception.TableName);
            Assert.AreEqual("The value for ColumnB in table TableA is required but has not been set", exception.Message);
        }

        [Test]
        public void IDictionaryExtensions_DictionaryWithMultipleRequiredPlaceholder_ThrowFirstError()
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "ColumnA", Placeholders.IsRequired() },
                { "ColumnB", Placeholders.IsRequired() }
            };
            RequiredPlaceholderIsNullException exception = Assert.Throws<RequiredPlaceholderIsNullException>(() => dictionary.ToSqlParameters());
            Assert.AreEqual("ColumnA", exception.ColumnName);
            Assert.AreEqual("The value for ColumnA is required but has not been set", exception.Message);
        }

        [Test]
        public void IDictionaryExtensions_DictionaryWithResolver_CallResolverAndReturnResult()
        {
            Func<int> resolveAction = () => 123;

            IDictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "ColumnA", new Resolver<int>(resolveAction) }
            };
            SqlParameter[] result = dictionary.ToSqlParameters();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("@ColumnA", result[0].ParameterName);
            Assert.AreEqual(123, result[0].Value);
        }

        [Test]
        public void IDictionaryExtensions_DictionaryWithNull_ReturnDBNull()
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "ColumnA", null }
            };
            SqlParameter[] result = dictionary.ToSqlParameters();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("@ColumnA", result[0].ParameterName);
            Assert.AreEqual(DBNull.Value, result[0].Value);
        }
    }
}
