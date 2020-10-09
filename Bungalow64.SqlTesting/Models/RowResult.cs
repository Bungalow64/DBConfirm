using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Dates.Abstract;
using Models.States.Abstract;
using Models.Strings.Abstract;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models
{
    public class RowResult
    {
        private readonly int _rowNumber;
        private readonly DataRow _row;
        private readonly QueryResult _queryResult;

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

        public RowResult AssertValue(string columnName, object expectedValue)
        {
            expectedValue = expectedValue ?? DBNull.Value;

            _queryResult.AssertColumnExists(columnName);

            object value = _row[columnName];

            if (expectedValue is IState stateValue)
            {
                stateValue.AssertState(value, $"Column {columnName} in row {_rowNumber} has an unexpected state");
            }
            else if (expectedValue is IDateComparison dateValue)
            {
                Assert.IsInstanceOfType(value, typeof(DateTime), $"Column {columnName} in row {_rowNumber} is not a valid DateTime object");

                dateValue.AssertDate((DateTime)value, $"Column {columnName} in row {_rowNumber} is different by {{0}}");
            }
            else if (expectedValue is IStringComparison stringValue)
            {
                Assert.IsInstanceOfType(value, typeof(string), $"Column {columnName} in row {_rowNumber} is not a valid String object");

                stringValue.AssertString((string)value, $"Column {columnName} in row {_rowNumber} has an unexpeccted length");
            }
            else
            {
                Assert.AreEqual(expectedValue, value, $"Column {columnName} in row {_rowNumber} has an unexpected value");
            }

            return this;
        }

        public RowResult AssertValues(DataSetRow expectedValues)
        {
            expectedValues = expectedValues ?? new DataSetRow();

            foreach (KeyValuePair<string, object> record in expectedValues)
            {
                AssertValue(record.Key, record.Value);
            }
            return this;
        }

        public RowResult ValidateRow(int rowNumber) =>
            new RowResult(_queryResult, rowNumber);
    }
}
