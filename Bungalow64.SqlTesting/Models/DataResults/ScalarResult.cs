using Models.TestFrameworks.Abstract;
using Models.Validation;

namespace Models.DataResults
{
    public class ScalarResult<T>
    {
        public T RawData { get; private set; }

        internal readonly ITestFramework TestFramework;

        public ScalarResult(ITestFramework testFramework, T rawData)
        {
            TestFramework = testFramework;
            RawData = rawData;
        }

        public ScalarResult<T> AssertValue(object expectedValue)
        {
            ValueValidation.Assert(TestFramework, expectedValue, RawData, $"Scalar result");

            return this;
        }
    }
}
