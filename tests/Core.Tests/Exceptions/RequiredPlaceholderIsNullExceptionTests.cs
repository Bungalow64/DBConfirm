using DBConfirm.Core.Exceptions;
using NUnit.Framework;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace SqlConfirm.Core.Tests.Exceptions
{
    [TestFixture]
    public class RequiredPlaceholderIsNullExceptionTests
    {
        [Test]
        public void RequiredPlaceholderIsNullException_DefaultCtor_MessageSet()
        {
            RequiredPlaceholderIsNullException exception = new RequiredPlaceholderIsNullException();
            Assert.AreEqual("The column is marked as required but has had no value set", exception.Message);
        }

        [Test]
        public void RequiredPlaceholderIsNullException_CtorWithColumnName_ColumnNameSet()
        {
            RequiredPlaceholderIsNullException exception = new RequiredPlaceholderIsNullException("ColumnA");
            Assert.AreEqual("The value for ColumnA is required but has not been set", exception.Message);
            Assert.AreEqual("ColumnA", exception.ColumnName);
        }

        [Test]
        public void RequiredPlaceholderIsNullException_CtorWithMessageAndException_MessageSet()
        {
            Exception inner = new ArgumentNullException();
            RequiredPlaceholderIsNullException exception = new RequiredPlaceholderIsNullException("Custom message", inner);
            Assert.AreEqual("Custom message", exception.Message);
            Assert.AreEqual(inner, exception.InnerException);
        }

        [Test]
        public void RequiredPlaceholderIsNullException_CtorWithColumnAndTableName_MessageAndColumnSet()
        {
            RequiredPlaceholderIsNullException exception = new RequiredPlaceholderIsNullException("ColumnA", "Table1");
            Assert.AreEqual("The value for ColumnA in table Table1 is required but has not been set", exception.Message);
            Assert.AreEqual("ColumnA", exception.ColumnName);
            Assert.AreEqual("Table1", exception.TableName);
        }

        [Test]
        public void RequiredPlaceholderIsNullException_CtorWithMessageAndColumnNameAndException_MessageAndColumnSet()
        {
            Exception inner = new ArgumentNullException();
            RequiredPlaceholderIsNullException exception = new RequiredPlaceholderIsNullException("Custom message", "ColumnA", inner);
            Assert.AreEqual("Custom message", exception.Message);
            Assert.AreEqual("ColumnA", exception.ColumnName);
            Assert.AreEqual(inner, exception.InnerException);
        }

        [Test]
        public void RequiredPlaceholderIsNullException_ToString_ReturnFullDetails()
        {
            RequiredPlaceholderIsNullException exception = new RequiredPlaceholderIsNullException("ColumnA");
            Assert.AreEqual(@"DBConfirm.Core.Exceptions.RequiredPlaceholderIsNullException: The value for ColumnA is required but has not been set
ColumnName: ColumnA", exception.ToString());
        }

        [Test]
        public void RequiredPlaceholderIsNullException_ToStringWithInnerException_ReturnFullDetails()
        {
            Exception inner = new ArgumentNullException();
            RequiredPlaceholderIsNullException exception = new RequiredPlaceholderIsNullException("Custom message", "ColumnA", inner);
            Assert.AreEqual(@"DBConfirm.Core.Exceptions.RequiredPlaceholderIsNullException: Custom message
ColumnName: ColumnA ---> System.ArgumentNullException: Value cannot be null.", exception.ToString());
        }

        [Test]
        public void RequiredPlaceholderIsNullException_SerialiseDeserialise_DataPersisted()
        {
            static byte[] SerializeToByteArray<T>(T obj) where T : class
            {
                if (obj == null)
                {
                    return null;
                }
                using MemoryStream ms = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(ms, obj);
                return ms.ToArray();
            }

            static T Deserialize<T>(byte[] byteArray) where T : class
            {
                if (byteArray == null)
                {
                    return default;
                }
                using MemoryStream memStream = new MemoryStream(byteArray);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(memStream);
            }

            RequiredPlaceholderIsNullException exception = new RequiredPlaceholderIsNullException("ColumnA");

            byte[] bytes = SerializeToByteArray(exception);
            RequiredPlaceholderIsNullException newException = Deserialize<RequiredPlaceholderIsNullException>(bytes);

            Assert.AreEqual("The value for ColumnA is required but has not been set", newException.Message);
            Assert.AreEqual("ColumnA", newException.ColumnName);
            Assert.AreEqual(@"DBConfirm.Core.Exceptions.RequiredPlaceholderIsNullException: The value for ColumnA is required but has not been set
ColumnName: ColumnA", newException.ToString());
        }

        [Test]
        public void RequiredPlaceholderIsNullException_SerialiseDeserialise_WithTableName_DataPersisted()
        {
            static byte[] SerializeToByteArray<T>(T obj) where T : class
            {
                if (obj == null)
                {
                    return null;
                }
                using MemoryStream ms = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(ms, obj);
                return ms.ToArray();
            }

            static T Deserialize<T>(byte[] byteArray) where T : class
            {
                if (byteArray == null)
                {
                    return default;
                }
                using MemoryStream memStream = new MemoryStream(byteArray);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(memStream);
            }

            RequiredPlaceholderIsNullException exception = new RequiredPlaceholderIsNullException("ColumnA", "Table1");

            byte[] bytes = SerializeToByteArray(exception);
            RequiredPlaceholderIsNullException newException = Deserialize<RequiredPlaceholderIsNullException>(bytes);

            Assert.AreEqual("The value for ColumnA in table Table1 is required but has not been set", newException.Message);
            Assert.AreEqual("ColumnA", newException.ColumnName);
            Assert.AreEqual("Table1", newException.TableName);
            Assert.AreEqual(@"DBConfirm.Core.Exceptions.RequiredPlaceholderIsNullException: The value for ColumnA in table Table1 is required but has not been set
ColumnName: ColumnA
TableName: Table1", newException.ToString());
        }
    }
}
