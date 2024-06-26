﻿using DBConfirm.Core.Data;
using DBConfirm.Core.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DBConfirm.Core.DataResults
{
    /// <summary>
    /// The data for a specific row
    /// </summary>
    public class RowResult
    {
        private readonly int _rowNumber;
        private readonly DataRow _row;
        private readonly QueryResult _queryResult;

        /// <summary>
        /// Constructor, setting the parent <see cref="QueryResult"/> and row number (zero-based).  Validates that the row number exists in the parent data set
        /// </summary>
        /// <param name="queryResult">The parent <see cref="QueryResult"/> object.  Must not be null</param>
        /// <param name="rowNumber">The row number (zero-based)</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RowResult(QueryResult queryResult, int rowNumber)
        {
            if (queryResult == null)
            {
                throw new ArgumentNullException(nameof(queryResult));
            }

            _row = queryResult.GetRow(rowNumber);
            _queryResult = queryResult;
            _rowNumber = rowNumber;
        }

        /// <summary>
        /// Constructor, setting the parent <see cref="QueryResult"/> and row number (zero-based).  Validates that the row number exists in the parent data set
        /// </summary>
        /// <param name="queryResult">The parent <see cref="QueryResult"/> object.  Must not be null</param>
        /// <param name="rowNumber">The row number (zero-based)</param>
        /// <param name="skipValidation">Whether the validation should be skipped</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal RowResult(QueryResult queryResult, int rowNumber, bool skipValidation)
        {
            if (queryResult == null)
            {
                throw new ArgumentNullException(nameof(queryResult));
            }

            _row = queryResult.GetRowIfPossible(rowNumber);
            _queryResult = queryResult;
            _rowNumber = rowNumber;
        }

        /// <summary>
        /// Asserts that a specific value exists for the given column.  Also asserts that the column exists
        /// </summary>
        /// <param name="columnName">The column name (case-sensitive)</param>
        /// <param name="expectedValue">The expected value.  Respects <see cref="Comparisons.Abstract.IComparison"/> objects</param>
        /// <returns>Returns the same <see cref="RowResult"/> object</returns>
        public RowResult AssertValue(string columnName, object expectedValue)
        {
            _queryResult.AssertColumnExists(columnName);

            object value = _row[columnName];

            ValueValidation.Assert(_queryResult.TestFramework, expectedValue, value, $"Column {columnName} in row {_rowNumber}");

            return this;
        }

        /// <summary>
        /// Asserts that the row matches the expected data.  Also asserts that all columns in the expected data exist
        /// </summary>
        /// <param name="expectedData">The expected data to match.  Respects <see cref="Comparisons.Abstract.IComparison"/> objects</param>
        /// <returns>Returns the same <see cref="RowResult"/> object</returns>
        public RowResult AssertValues(DataSetRow expectedData)
        {
            expectedData = expectedData ?? new DataSetRow();

            foreach (KeyValuePair<string, object> record in expectedData)
            {
                AssertValue(record.Key, record.Value);
            }
            return this;
        }

        /// <summary>
        /// Returns a <see cref="RowResult"/> object, representing the specific row on which further assertions can be made.  Validates that the row number exists in the parent data set
        /// </summary>
        /// <param name="rowNumber">The row number (zero-based)</param>
        /// <returns>Returns the <see cref="RowResult"/> for the row</returns>
        public RowResult ValidateRow(int rowNumber) =>
            new RowResult(_queryResult, rowNumber);

        /// <summary>
        /// Validates whether the values match the row, returning a boolean representing the result
        /// </summary>
        /// <param name="expectedData">The expected data to match.  Respects <see cref="Comparisons.Abstract.IComparison"/> objects</param>
        /// <returns>Returns whether the values match the current row</returns>
        internal bool ValidateValuesMatch(DataSetRow expectedData)
        {
            expectedData = expectedData ?? new DataSetRow();

            return expectedData.All(p => ValidateValue(p.Key, p.Value));
        }

        /// <summary>
        /// Checks that the row matches the expected data, returning a boolean as the result instead of failing the test
        /// </summary>
        /// <param name="expectedData">The expected data to match.  Respects <see cref="Comparisons.Abstract.IComparison"/> objects</param>
        /// <returns>Returns a boolean indicating if the values match</returns>
        internal bool DoValuesMatch(DataSetRow expectedData)
        {
            expectedData = expectedData ?? new DataSetRow();

            return expectedData.All(p => ValidateValueNoAssertions(p.Key, p.Value));
        }

        /// <summary>
        /// Gets the value in a specific column
        /// </summary>
        /// <param name="columnName">The column name (case-sensitive)</param>
        /// <returns>The object in the column</returns>
        internal object Get(string columnName)
        {
            return _row[columnName];
        }

        /// <summary>
        /// Gets the current row number (0-based)
        /// </summary>
        /// <returns>Returns the row number (0-based)</returns>
        internal int GetRowNumber()
        {
            return _rowNumber;
        }

        private bool ValidateValue(string columnName, object expectedValue)
        {
            _queryResult.AssertColumnExists(columnName);

            object value = _row[columnName];

            return ValueValidation.Validate(expectedValue, value);
        }

        private bool ValidateValueNoAssertions(string columnName, object expectedValue)
        {
            if (!_queryResult.CheckColumnExists(columnName))
            {
                return false;
            }

            if (_row is null)
            {
                return false;
            }

            object value = _row[columnName];

            return ValueValidation.Validate(expectedValue, value);
        }
    }
}
