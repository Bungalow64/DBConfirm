using System;
using Xunit.Sdk;

namespace DBConfirm.Frameworks.XUnit
{
    public class XUnitException : XunitException
    {
        public static string HandleMessage(string userMessage, Exception innerException)
        {
            var exMessage = innerException.Message;
            var i = exMessage.IndexOf("\r\n");
            exMessage = exMessage.Substring(i, exMessage.Length - i);

            return $"{userMessage}{exMessage}";
        }

        public XUnitException(string userMessage)
            : base(userMessage)
        {
        }

        public XUnitException(string userMessage, Exception innerException)
            : base(HandleMessage(userMessage, innerException))
        {
        }
    }
}