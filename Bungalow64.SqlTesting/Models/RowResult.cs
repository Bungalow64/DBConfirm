using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Dates.Abstract;
using Models.States.Abstract;
using Models.Strings.Abstract;
using Models.Validation;
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
            _queryResult.AssertColumnExists(columnName);

            object value = _row[columnName];

            ValueValidation.Validate(expectedValue, value, $"Column {columnName} in row {_rowNumber}");

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
