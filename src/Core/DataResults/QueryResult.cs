using DBConfirm.Core.Data;
using DBConfirm.Core.TestFrameworks.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DBConfirm.Core.DataResults
{
    /// <summary>
    /// The results of a query execution, representing the data returned
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        /// The data returned from the query execution
        /// </summary>
        public DataTable RawData { get; private set; }

        /// <summary>
        /// The test framework to use for assertions
        /// </summary>
        internal readonly ITestFramework TestFramework;

        /// <summary>
        /// Constructor, including the test framework to use
        /// </summary>
        /// <param name="testFramework">The test framework to use for assertions</param>
        public QueryResult(ITestFramework testFramework)
        {
            TestFramework = testFramework;
            RawData = new DataTable();
        }

        /// <summary>
        /// Constructor, including the test framework to use and the query result data
        /// </summary>
        /// <param name="testFramework">The test framework to use for assertions</param>
        /// <param name="rawData">The data returned from the query execution</param>
        public QueryResult(ITestFramework testFramework, DataTable rawData)
        {
            TestFramework = testFramework;
            RawData = rawData ?? new DataTable();
        }

        /// <summary>
        /// The total number of rows in the data set
        /// </summary>
        public int TotalRows => RawData.Rows.Count;
        /// <summary>
        /// The total number of columns in the data set
        /// </summary>
        public int TotalColumns => RawData.Columns.Count;

        /// <summary>
        /// The collection of columns in the data set, in the order they appear in the data set
        /// </summary>
        public ICollection<string> ColumnNames => RawData.Columns.Cast<DataColumn>().Select(p => p.ColumnName).ToList();

        /// <summary>
        /// Asserts the number of rows
        /// </summary>
        /// <param name="expected">The expected number of rows</param>
        /// <returns>Returns the same <see cref="QueryResult"/> object</returns>
        public QueryResult AssertRowCount(int expected)
        {
            TestFramework.AreEqual(expected, TotalRows, $"The total row count is unexpected");
            return this;
        }

        /// <summary>
        /// Asserts the number of columns
        /// </summary>
        /// <param name="expected">The expected number of columns</param>
        /// <returns>Returns the same <see cref="QueryResult"/> object</returns>
        public QueryResult AssertColumnCount(int expected)
        {
            TestFramework.AreEqual(expected, TotalColumns, $"The total column count is unexpected");
            return this;
        }

        /// <summary>
        /// Asserts that a specific column exists in the data set
        /// </summary>
        /// <param name="expectedColumnName">The column name (case-sensitive)</param>
        /// <returns>Returns the same <see cref="QueryResult"/> object</returns>
        public QueryResult AssertColumnExists(string expectedColumnName)
        {
            string GetFailureMessage()
            {
                if (RawData.Columns.Count == 0)
                {
                    return $"Expected column {expectedColumnName ?? "<null>"} to be found but no columns were found";
                }
                if (RawData.Columns.Count == 1)
                {
                    return $"Expected column {expectedColumnName ?? "<null>"} to be found but the only column found is {ColumnNames.First()}";
                }
                return $"Expected column {expectedColumnName ?? "<null>"} to be found but the only columns found are {string.Join(", ", ColumnNames)}";
            };

            TestFramework.Contains(ColumnNames.ToList(), expectedColumnName, GetFailureMessage());
            return this;
        }

        /// <summary>
        /// Asserts that a specific column does not exist in the data set
        /// </summary>
        /// <param name="notExpectedColumnName">The column name (case-sensitive)</param>
        /// <returns>Returns the same <see cref="QueryResult"/> object</returns>
        public QueryResult AssertColumnNotExists(string notExpectedColumnName)
        {
            TestFramework.DoesNotContain(ColumnNames.ToList(), notExpectedColumnName, $"Expected column {notExpectedColumnName} to not be found but it was found");
            return this;
        }

        /// <summary>
        /// Asserts that a number of columns all exist in the data set
        /// </summary>
        /// <param name="expectedColumnNames">The column names (case-sensitive)</param>
        /// <returns>Returns the same <see cref="QueryResult"/> object</returns>
        public QueryResult AssertColumnsExist(params string[] expectedColumnNames)
        {
            (expectedColumnNames ?? new string[] { null })
                .ToList()
                .ForEach(p => AssertColumnExists(p));
            return this;
        }

        /// <summary>
        /// Asserts that a number of columns all do not exist in the data set
        /// </summary>
        /// <param name="notExpectedColumnNames">The column names (case-sensitive)</param>
        /// <returns>Returns the same <see cref="QueryResult"/> object</returns>
        public QueryResult AssertColumnsNotExist(params string[] notExpectedColumnNames)
        {
            (notExpectedColumnNames ?? new string[] { null })
                .ToList()
                .ForEach(p => AssertColumnNotExists(p));
            return this;
        }

        /// <summary>
        /// Asserts that a row exists at a specific position (zero-based)
        /// </summary>
        /// <param name="expectedRowPosition">The row position (zero-based)</param>
        /// <returns>Returns the same <see cref="QueryResult"/> object</returns>
        public QueryResult AssertRowPositionExists(int expectedRowPosition)
        {
            TestFramework.IsTrue(IsRowFound(expectedRowPosition), $"There is no row at position {expectedRowPosition} (zero-based).  There {(TotalRows == 1 ? "is 1 row" : $"are {TotalRows} rows")}");
            return this;
        }

        /// <summary>
        /// Asserts that a specific value exists for the given row and column.  Also asserts that the row and column exists
        /// </summary>
        /// <param name="rowNumber">The row number (zero-based)</param>
        /// <param name="columnName">The column name (case-sensitive)</param>
        /// <param name="expectedValue">The expected value.  Respects <see cref="Comparisons.Abstract.IComparison"/> objects</param>
        /// <returns>Returns the same <see cref="QueryResult"/> object</returns>
        public QueryResult AssertValue(int rowNumber, string columnName, object expectedValue)
        {
            ValidateRow(rowNumber)
                .AssertValue(columnName, expectedValue);

            return this;
        }

        /// <summary>
        /// Returns a <see cref="RowResult"/> object, representing the specific row on which further assertions can be made.  Validates that the row number exists in the data set
        /// </summary>
        /// <param name="rowNumber">The row number (zero-based)</param>
        /// <returns>Returns the <see cref="RowResult"/> for the row</returns>
        public RowResult ValidateRow(int rowNumber) =>
            new RowResult(this, rowNumber);

        /// <summary>
        /// Asserts that the row at the given position matches the expected data.  Also asserts that all columns in the expected data exist
        /// </summary>
        /// <param name="rowNumber">The row number (zero-based)</param>
        /// <param name="expectedData">The expected data to match.  Respects <see cref="Comparisons.Abstract.IComparison"/> objects</param>
        /// <returns>Returns the same <see cref="QueryResult"/> object</returns>
        public QueryResult AssertRowValues(int rowNumber, DataSetRow expectedData)
        {
            ValidateRow(rowNumber)
                .AssertValues(expectedData);
            return this;
        }

        /// <summary>
        /// Asserts that at least one row matches the expected data.  Also asserts that all columns in the expected data exist
        /// </summary>
        /// <param name="expectedData">The expected data to match.  Respects <see cref="Comparisons.Abstract.IComparison"/> objects</param>
        /// <returns>Returns the same <see cref="QueryResult"/> object</returns>
        public QueryResult AssertRowExists(DataSetRow expectedData)
        {
            AssertColumnNames(expectedData);

            for (int x = 0; x < TotalRows; x++)
            {
                if (ValidateRow(x).ValidateValuesMatch(expectedData))
                {
                    return this;
                }
            }

            TestFramework.Fail($"No rows found matching the expected data: {expectedData}");
            return this;
        }

        /// <summary>
        /// Asserts that no rows match the supplied data.  Also asserts that all columns in the supplied data exist
        /// </summary>
        /// <param name="unexpectedData">The unexpected data.  Respects <see cref="Comparisons.Abstract.IComparison"/> objects</param>
        /// <returns>Returns the same <see cref="QueryResult"/> object</returns>
        public QueryResult AssertRowDoesNotExist(DataSetRow unexpectedData)
        {
            AssertColumnNames(unexpectedData);

            for (int x = 0; x < TotalRows; x++)
            {
                if (CheckRowValues(x, unexpectedData))
                {
                    TestFramework.Fail($"Row {x} matches the expected data that should not match anything: {unexpectedData}");
                }
            }

            return this;
        }

        /// <summary>
        /// Gets the data row for the specific position, after asserting that the row position exists
        /// </summary>
        /// <param name="rowNumber">The row number (zero-based)</param>
        /// <returns>Returns the data row</returns>
        internal DataRow GetRow(int rowNumber)
        {
            AssertRowPositionExists(rowNumber);
            return RawData.Rows[rowNumber];
        }

        /// <summary>
        /// Gets the data row for the specific position, if the row exists.  If it doesn't exist, null is returned
        /// </summary>
        /// <param name="rowNumber">The row number (zero-based)</param>
        /// <returns>Returns the data row, or null if the row isn't found</returns>
        internal DataRow GetRowIfPossible(int rowNumber)
        {
            if (IsRowFound(rowNumber))
            {
                return RawData.Rows[rowNumber];
            }

            return null;
        }

        /// <summary>
        /// Checks that a specific column exists in the data set, returning a boolean as the result
        /// </summary>
        /// <param name="expectedColumnName">The column name (case-sensitive)</param>
        /// <returns>Returns a boolean indicating whether the column exists</returns>
        internal bool CheckColumnExists(string expectedColumnName)
        {
            return ColumnNames.Any(p => p.Equals(expectedColumnName));
        }

        private void AssertColumnNames(DataSetRow expectedData)
        {
            foreach (KeyValuePair<string, object> row in expectedData)
            {
                AssertColumnExists(row.Key);
            }
        }

        private bool CheckRowValues(int rowNumber, DataSetRow expectedData)
        {
            var row = new RowResult(this, rowNumber);
            return row.DoValuesMatch(expectedData);
        }

        private bool IsRowFound(int expectedRowPosition)
        {
            return TotalRows > expectedRowPosition && expectedRowPosition >= 0;
        }
    }
}
