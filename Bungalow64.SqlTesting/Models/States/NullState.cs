using Models.States.Abstract;
using Models.TestFrameworks.Abstract;
using System;

namespace Models.States
{
    public class NullState : IState
    {
        public void AssertState(ITestFramework testFramework, object value, string message)
        {
            testFramework.Assert.AreEqual(DBNull.Value, value ?? DBNull.Value, message);
        }
    }
}
