using DBConfirm.Core.Comparisons.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;

namespace DBConfirm.Core.Comparisons.Numeric
{
    /// <summary>
    /// Asserts that a numeric value matches another, regardless of the actual types of the values
    /// </summary>
    public class NumericValue : IComparison
    {
        /// <summary>
        /// The expected value
        /// </summary>
        public object ExpectedValue { get; }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public NumericValue(int expectedValue) : this((object)expectedValue)
        {
        }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public NumericValue(short expectedValue) : this((object)expectedValue)
        {
        }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public NumericValue(long expectedValue) : this((object)expectedValue)
        {
        }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public NumericValue(decimal expectedValue) : this((object)expectedValue)
        {
        }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public NumericValue(double expectedValue) : this((object)expectedValue)
        {
        }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public NumericValue(float expectedValue) : this((object)expectedValue)
        {
        }

        /// <summary>
        /// Constructor, setting the expected value
        /// </summary>
        /// <param name="expectedValue">The expected value</param>
        public NumericValue(byte expectedValue) : this((object)expectedValue)
        {
        }

        private NumericValue(object expectedValue)
        {
            ExpectedValue = expectedValue;
        }

        /// <inheritdoc/>
        public void Assert(ITestFramework testFramework, object value, string messagePrefix)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (value?.GetType() == ExpectedValue?.GetType())
            {
                testFramework.AreEqual(ExpectedValue, value, $"{messagePrefix} does not match expected value");
            }

            (object result, bool isCast) = TryCast(value);

            if (isCast)
            {
                testFramework.AreEqual(result, value, $"{messagePrefix} does not match expected value");
            }
            else
            {
                testFramework.AreEqual(ExpectedValue, value, $"{messagePrefix} does not match expected value");
            }
        }

        /// <inheritdoc/>
        public bool Validate(object value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            (object result, bool isCast) = TryCast(value);

            if (isCast)
            {
                return value.Equals(result);
            }
            else
            {
                return value.Equals(ExpectedValue);
            }
        }

        private (object result, bool isCast) TryCast(object actualValue)
        {
            switch (actualValue)
            {
                case int _:
                    if (int.TryParse(ExpectedValue.ToString(), out int expectedValueInt))
                    {
                        return (expectedValueInt, true);
                    }
                    break;
                case short _:
                    if (short.TryParse(ExpectedValue.ToString(), out short expectedValueShort))
                    {
                        return (expectedValueShort, true);
                    }
                    break;
                case long _:
                    if (long.TryParse(ExpectedValue.ToString(), out long expectedValueLong))
                    {
                        return (expectedValueLong, true);
                    }
                    break;
                case decimal _:
                    if (decimal.TryParse(ExpectedValue.ToString(), out decimal expectedValueDecimal))
                    {
                        return (expectedValueDecimal, true);
                    }
                    break;
                case double _:
                    if (double.TryParse(ExpectedValue.ToString(), out double expectedValueDouble))
                    {
                        return (expectedValueDouble, true);
                    }
                    break;
                case float _:
                    if (float.TryParse(ExpectedValue.ToString(), out float expectedValueFloat))
                    {
                        return (expectedValueFloat, true);
                    }
                    break;
                case byte _:
                    if (byte.TryParse(ExpectedValue.ToString(), out byte expectedValueByte))
                    {
                        return (expectedValueByte, true);
                    }
                    break;
            }

            return (null, false);
        }
    }
}
