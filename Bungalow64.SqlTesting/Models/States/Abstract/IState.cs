using Models.TestFrameworks.Abstract;

namespace Models.States.Abstract
{
    public interface IState
    {
        void AssertState(ITestFramework testFramework, object value, string message);
        bool Validate(object value);
    }
}
