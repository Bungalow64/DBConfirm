using Models.DataResults;
using NUnit.Framework;
using System.Data;

namespace Models.Tests.DataResults
{
    [TestFixture]
    public class RowResultTests
    {
        private DataTable CreateDefaultTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("UserId", typeof(int));
            table.Columns.Add("DomainId", typeof(int));
            return table;
        }
        private void AddRow(DataTable table, int userId, int domainId)
        {
            DataRow row = table.NewRow();
            row["UserId"] = userId;
            row["DomainId"] = domainId;
            table.Rows.Add(row);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void RowResult_RowExists_NoError(int rowNumber)
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);
            AddRow(table, 2001, 2002);
            AddRow(table, 3001, 3002);

            QueryResult queryResult = new QueryResult(table);
            Assert.DoesNotThrow(() => { RowResult result = new RowResult(queryResult, rowNumber); });
        }

        [TestCase(-1)]
        [TestCase(3)]
        public void RowResult_RowDoesNotExist_Error(int rowNumber)
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);
            AddRow(table, 2001, 2002);
            AddRow(table, 3001, 3002);

            QueryResult queryResult = new QueryResult(table);
            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                { RowResult result = new RowResult(queryResult, rowNumber); });

            Assert.AreEqual($"Assert.IsTrue failed. There is no row at position {rowNumber} (zero-based).  There are 3 rows", exception.Message);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void RowResult_ValidateRow_RowExists_NoError(int rowNumber)
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);
            AddRow(table, 2001, 2002);
            AddRow(table, 3001, 3002);

            RowResult result = new RowResult(new QueryResult(table), 0);

            Assert.DoesNotThrow(() => { RowResult nextResult = result.ValidateRow(rowNumber); });
        }

        [TestCase(-1)]
        [TestCase(3)]
        public void RowResult_ValidateRow_RowDoesNotExist_Error(int rowNumber)
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);
            AddRow(table, 2001, 2002);
            AddRow(table, 3001, 3002);

            RowResult result = new RowResult(new QueryResult(table), 0);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                { RowResult nextResult = result.ValidateRow(rowNumber); });

            Assert.AreEqual($"Assert.IsTrue failed. There is no row at position {rowNumber} (zero-based).  There are 3 rows", exception.Message);
        }


        [TestCase(0, 1001)]
        [TestCase(1, 2001)]
        [TestCase(2, 3001)]
        public void RowResult_AssertValue_ValueMatches(int rowNumber, int expectedUserId)
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);
            AddRow(table, 2001, 2002);
            AddRow(table, 3001, 3002);

            RowResult result = new RowResult(new QueryResult(table), rowNumber);

            Assert.DoesNotThrow(() => { result = result.AssertValue("UserId", expectedUserId); });
        }

        [TestCase(0, 1002)]
        [TestCase(0, 2002)]
        [TestCase(1, 2002)]
        [TestCase(1, 3002)]
        [TestCase(2, 3002)]
        [TestCase(2, 4002)]
        public void RowResult_AssertValue_ValueDoesNotMatch_Error(int rowNumber, int expectedUserId)
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);
            AddRow(table, 2001, 2002);
            AddRow(table, 3001, 3002);

            RowResult result = new RowResult(new QueryResult(table), rowNumber);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            { result = result.AssertValue("UserId", expectedUserId); });

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<{ expectedUserId }>. Actual:<{table.Rows[rowNumber]["UserId"]}>. Column UserId in row {rowNumber} has an unexpected value", exception.Message);
        }

        [Test]
        public void RowResult_AssertValue_ColumnDoesNotExist_Error()
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);

            RowResult result = new RowResult(new QueryResult(table), 0);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            { result = result.AssertValue("UserId2", 1001); });

            Assert.AreEqual("CollectionAssert.Contains failed. Expected column UserId2 to be found but the only columns found are UserId, DomainId", exception.Message);
        }

        [Test]
        public void RowResult_AssertValue_ColumnNull_Error()
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);

            RowResult result = new RowResult(new QueryResult(table), 0);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            { result = result.AssertValue(null, 1001); });

            Assert.AreEqual($"CollectionAssert.Contains failed. Expected column <null> to be found but the only columns found are UserId, DomainId", exception.Message);
        }

        [Test]
        public void RowResult_AssertValue_TableHasNoColumns_RequestColumn_Error()
        {
            DataTable table = new DataTable();
            DataRow row = table.NewRow();
            table.Rows.Add(row);

            RowResult result = new RowResult(new QueryResult(table), 0);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            { result = result.AssertValue("UserId", 1001); });

            Assert.AreEqual($"CollectionAssert.Contains failed. Expected column UserId to be found but no columns were found", exception.Message);
        }

        [Test]
        public void RowResult_AssertValue_TableHasNoColumns_RequestNullColumn_Error()
        {
            DataTable table = new DataTable();
            DataRow row = table.NewRow();
            table.Rows.Add(row);

            RowResult result = new RowResult(new QueryResult(table), 0);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            { result = result.AssertValue(null, 1001); });

            Assert.AreEqual($"CollectionAssert.Contains failed. Expected column <null> to be found but no columns were found", exception.Message);
        }

        [TestCase(0, 1001, 1002)]
        [TestCase(1, 2001, 2002)]
        [TestCase(2, 3001, 3002)]
        public void RowResult_AssertValues_ValuesMatch(int rowNumber, int expectedUserId, int expectedDomainId)
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);
            AddRow(table, 2001, 2002);
            AddRow(table, 3001, 3002);

            RowResult result = new RowResult(new QueryResult(table), rowNumber);

            DataSetRow expectedData = new DataSetRow
            {
                { "UserId", expectedUserId },
                { "DomainId", expectedDomainId }
            };

            Assert.DoesNotThrow(() => { result = result.AssertValues(expectedData); });
        }

        [TestCase(0, 1002, 1002)]
        [TestCase(0, 2002, 1002)]
        [TestCase(1, 2002, 2002)]
        [TestCase(1, 3002, 2002)]
        [TestCase(2, 3002, 3002)]
        [TestCase(2, 4002, 3002)]
        public void RowResult_AssertValues_FirstColumnValuesDoNotMatch_Error(int rowNumber, int expectedUserId, int expectedDomainId)
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);
            AddRow(table, 2001, 2002);
            AddRow(table, 3001, 3002);

            DataSetRow expectedData = new DataSetRow
            {
                { "UserId", expectedUserId },
                { "DomainId", expectedDomainId }
            };

            RowResult result = new RowResult(new QueryResult(table), rowNumber);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
                { result = result.AssertValues(expectedData); });

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<{ expectedUserId }>. Actual:<{table.Rows[rowNumber]["UserId"]}>. Column UserId in row {rowNumber} has an unexpected value", exception.Message);
        }

        [TestCase(0, 1001, 4001)]
        [TestCase(1, 2001, 4001)]
        [TestCase(2, 3001, 4001)]
        public void RowResult_AssertValues_SecondColumnValuesDoNotMatch_Error(int rowNumber, int expectedUserId, int expectedDomainId)
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);
            AddRow(table, 2001, 2002);
            AddRow(table, 3001, 3002);

            DataSetRow expectedData = new DataSetRow
            {
                { "UserId", expectedUserId },
                { "DomainId", expectedDomainId }
            };

            RowResult result = new RowResult(new QueryResult(table), rowNumber);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            { result = result.AssertValues(expectedData); });

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<{ expectedDomainId }>. Actual:<{table.Rows[rowNumber]["DomainId"]}>. Column DomainId in row {rowNumber} has an unexpected value", exception.Message);
        }

        [TestCase(0, 4001, 5001)]
        [TestCase(1, 4001, 5001)]
        [TestCase(2, 4001, 5001)]
        public void RowResult_AssertValues_BothColumnValuesDoNotMatch_ShowFirstError(int rowNumber, int expectedUserId, int expectedDomainId)
        {
            DataTable table = CreateDefaultTable();

            AddRow(table, 1001, 1002);
            AddRow(table, 2001, 2002);
            AddRow(table, 3001, 3002);

            DataSetRow expectedData = new DataSetRow
            {
                { "UserId", expectedUserId },
                { "DomainId", expectedDomainId }
            };

            RowResult result = new RowResult(new QueryResult(table), rowNumber);

            var exception = Assert.Throws<Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException>(() =>
            { result = result.AssertValues(expectedData); });

            Assert.AreEqual($"Assert.AreEqual failed. Expected:<{ expectedUserId }>. Actual:<{table.Rows[rowNumber]["UserId"]}>. Column UserId in row {rowNumber} has an unexpected value", exception.Message);
        }
    }
}
