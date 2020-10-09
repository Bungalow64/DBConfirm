using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.States.Abstract;
using System;

namespace Models.States
{
    public class NotNullState : IState
    {
        public void AssertState(object value, string message)
        {
            Assert.AreNotEqual(DBNull.Value, value ?? DBNull.Value, message);
        }
    }
}
