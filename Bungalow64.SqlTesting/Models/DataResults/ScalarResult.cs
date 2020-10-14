using Models.Validation;

namespace Models.DataResults
{
    public class ScalarResult<T>
    {
        public T RawData { get; private set; }

        public ScalarResult(T rawData)
        {
            RawData = rawData;
        }

        public ScalarResult<T> AssertValue(object expectedValue)
        {
            ValueValidation.Validate(expectedValue, RawData, $"Scalar result");

            return this;
        }
    }
}
