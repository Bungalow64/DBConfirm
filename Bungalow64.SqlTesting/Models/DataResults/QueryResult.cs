using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Models.DataResults
{
    public class QueryResult
    {
        public DataTable RawData { get; private set; }

        public QueryResult()
        {
            RawData = new DataTable();
        }

        public QueryResult(DataTable rawData)
        {
            RawData = rawData ?? new DataTable();
        }

        public int TotalRows => RawData.Rows.Count;
        public int TotalColumns => RawData.Columns.Count;

        public ICollection<string> ColumnNames => RawData.Columns.Cast<DataColumn>().Select(p => p.ColumnName).ToList();

        public QueryResult AssertRowCount(int expected)
        {
            Assert.AreEqual(expected, TotalRows, $"The total row count is unexpected");
            return this;
        }

        public QueryResult AssertColumnCount(int expected)
        {
            Assert.AreEqual(expected, TotalColumns, $"The total column count is unexpected");
            return this;
        }

        public QueryResult AssertColumnExists(string expectedColumnName)
        {
            string GetFailureMessage()
            {
                if (RawData.Columns.Count == 0)
                {
                    return $"Expected column {expectedColumnName ?? "<null>"} to be found but no columns were found";
                }
                return $"Expected column {expectedColumnName ?? "<null>"} to be found but the only columns found are {string.Join(", ", ColumnNames)}";
            };

            CollectionAssert.Contains(ColumnNames.ToList(), expectedColumnName, GetFailureMessage());
            return this;
        }

        public QueryResult AssertColumnNotExists(string expectedColumnName)
        {
            CollectionAssert.DoesNotContain(ColumnNames.ToList(), expectedColumnName, $"Expected column {expectedColumnName} to not be found but it was found");
            return this;
        }

        public QueryResult AssertColumnsExist(params string[] columnNames)
        {
            (columnNames ?? new string[] { null })
                .ToList()
                .ForEach(p => AssertColumnExists(p));
            return this;
        }

        public QueryResult AssertColumnsNotExist(params string[] columnNames)
        {
            (columnNames ?? new string[] { null })
                .ToList()
                .ForEach(p => AssertColumnNotExists(p));
            return this;
        }

        public QueryResult AssertRowPositionExists(int expectedRowNumber)
        {
            Assert.IsTrue(TotalRows > expectedRowNumber && expectedRowNumber >= 0, $"There is no row at position {expectedRowNumber} (zero-based).  There {(TotalRows == 1 ? "is 1 row" : $"are {TotalRows} rows")}");
            return this;
        }

        public QueryResult AssertValue(int rowNumber, string columnName, object expectedValue)
        {
            ValidateRow(rowNumber)
                .AssertValue(columnName, expectedValue);

            return this;
        }

        public RowResult ValidateRow(int rowNumber) =>
            new RowResult(this, rowNumber);

        public QueryResult AssertRowValues(int rowNumber, DataSetRow expectedData)
        {
            ValidateRow(rowNumber)
                .AssertValues(expectedData);
            return this;
        }

        public QueryResult AssertRowExists(DataSetRow expectedData)
        {
            AssertColumnNames(expectedData);

            for (int x = 0; x < TotalRows; x++)
            {
                try
                {
                    AssertRowValues(x, expectedData);
                    return this;
                }
                catch (AssertFailedException) { }
            }

            Assert.Fail($"No rows found matching the expected data: {expectedData}");
            return this;
        }

        public QueryResult AssertRowDoesNotExist(DataSetRow expectedData)
        {
            AssertColumnNames(expectedData);

            for (int x = 0; x < TotalRows; x++)
            {
                bool isMatch = false;
                try
                {
                    AssertRowValues(x, expectedData);
                    isMatch = true;
                }
                catch (AssertFailedException) { }

                if (isMatch)
                {
                    Assert.Fail($"Row {x} matches the expected data that should not match anything: {expectedData}");
                }
            }

            return this;
        }

        internal DataRow GetRow(int rowNumber)
        {
            AssertRowPositionExists(rowNumber);
            return RawData.Rows[rowNumber];
        }

        private void AssertColumnNames(DataSetRow expectedData)
        {
            foreach (KeyValuePair<string, object> row in expectedData)
            {
                AssertColumnExists(row.Key);
            }
        }
    }
}
