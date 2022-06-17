using DBConfirm.Core.Data;
using DBConfirm.Core.Templates;

namespace Sample.Core.MySQL.MSTest.Tests.Templates
{
    public class NumbersTableTemplate : BaseIdentityTemplate<NumbersTableTemplate>
    {
        public override string TableName => "`NumbersTable`";
        
        public override string IdentityColumnName => "Id";

        public override DataSetRow DefaultData => new DataSetRow
        {
            
        };

        public NumbersTableTemplate WithId(int value) => SetValue("Id", value);
        public NumbersTableTemplate WithIntColumn(int value) => SetValue("IntColumn", value);
        public NumbersTableTemplate WithSmallIntColumn(int value) => SetValue("SmallIntColumn", value);
        public NumbersTableTemplate WithBigIntColumn(long value) => SetValue("BigIntColumn", value);
        public NumbersTableTemplate WithDecimalColumn(decimal value) => SetValue("DecimalColumn", value);
        public NumbersTableTemplate WithMoneyColumn(decimal value) => SetValue("MoneyColumn", value);
        public NumbersTableTemplate WithSmallMoneyColumn(decimal value) => SetValue("SmallMoneyColumn", value);
        public NumbersTableTemplate WithNumericColumn(decimal value) => SetValue("NumericColumn", value);
        public NumbersTableTemplate WithFloatColumn(double value) => SetValue("FloatColumn", value);
        public NumbersTableTemplate WithRealColumn(double value) => SetValue("RealColumn", value);
        public NumbersTableTemplate WithTinyIntColumn(byte value) => SetValue("TinyIntColumn", value);
    }
}