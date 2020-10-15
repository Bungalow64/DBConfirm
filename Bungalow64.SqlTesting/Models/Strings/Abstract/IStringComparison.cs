using Models.TestFrameworks.Abstract;

namespace Models.Strings.Abstract
{
    public interface IStringComparison
    {
        void AssertString(ITestFramework testFramework, string value, string message);
    }
}
