using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.States.Abstract;
using System;

namespace Models.States
{
    public class NullState : IState
    {
        public void AssertState(object value, string message)
        {
            Assert.AreEqual(DBNull.Value, value ?? DBNull.Value, message);
        }
    }
}
