using System;
using Xunit.Sdk;

namespace DBConfirm.Frameworks.XUnit
{
    /// <summary>
    /// <see cref="XunitException"/> wrapper
    /// </summary>
    public class XUnitException : XunitException
    {
        /// <summary>
        /// Removes the first line of the <see cref="innerException"/> and appends the rest to the <see cref="userMessage"/>.
        /// The first line of an <see cref="XunitException"/> includes the `Assert.*` that was called internally.
        /// </summary>
        private static string HandleMessage(string userMessage, Exception innerException)
        {
            var exMessage = innerException?.Message ?? "";
            var i = exMessage.IndexOf("\r\n");
            if (i > 0)
                exMessage = exMessage.Substring(i, exMessage.Length - i);

            return $"{userMessage}{exMessage}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitException"/> class.
        /// </summary>
        /// <param name="userMessage">The user message to be displayed</param>
        public XUnitException(string userMessage)
            : base(userMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitException"/> class.
        /// </summary>
        /// <param name="userMessage">The user message to be displayed</param>
        /// <param name="innerException">The inner exception, the first line of the Exception will be removed</param>
        public XUnitException(string userMessage, Exception innerException)
            : base(HandleMessage(userMessage, innerException), innerException?.InnerException)
        {
        }
    }
}