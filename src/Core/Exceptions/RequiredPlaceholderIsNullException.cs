using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SQLConfirm.Core.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a required placeholder has not been replaced by a value
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class RequiredPlaceholderIsNullException : Exception
    {
        private readonly string _columnName;
        private const string _message = "The column is marked as required but has had no value set";

        /// <summary>
        /// The name of the required column missing a value
        /// </summary>
        public string ColumnName => _columnName;

        /// <summary>
        /// Initializes a new instance of the RequiredPlaceholderIsNullException class
        /// </summary>
        public RequiredPlaceholderIsNullException()
            : base(_message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RequiredPlaceholderIsNullException class with the name of the column missing a value
        /// </summary>
        /// <param name="columnName">The name of the column missing a value</param>
        public RequiredPlaceholderIsNullException(string columnName)
            : base()
        {
            _columnName = columnName;
        }

        /// <summary>
        /// Initializes a new instance of the RequiredPlaceholderIsNullException class with a specified error message and the exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public RequiredPlaceholderIsNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RequiredPlaceholderIsNullException class with a specified error message that explains the reason for this exception
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception</param>
        /// <param name="columnName">The name of the column missing a value</param>
        public RequiredPlaceholderIsNullException(string message, string columnName)
            : base(message)
        {
            _columnName = columnName;
        }

        /// <summary>
        /// Initializes a new instance of the RequiredPlaceholderIsNullException class with a specified error message and the exception that is the cause of this exception, and the name of the column missing a value
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception</param>
        /// <param name="columnName">The name of the column missing a value</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public RequiredPlaceholderIsNullException(string message, string columnName, Exception innerException)
            : base(message, innerException)
        {
            _columnName = columnName;
        }

        /// <summary>
        /// Initializes a new instance of the RequiredPlaceholderIsNullException class with serialized data
        /// </summary>
        /// <param name="info">The object that holds the serialized object data</param>
        /// <param name="context">An object that describes the source or destination of the serialized data</param>
        protected RequiredPlaceholderIsNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _columnName = info.GetString("RequiredPlaceholderIsNull_ColumnName");
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            string s = GetType().FullName + ": " + Message;

            if (_columnName != null && _columnName.Length != 0)
            {
                s += Environment.NewLine + "ColumnName: " + _columnName;
            }

            if (InnerException != null)
            {
                s = s + " ---> " + InnerException.ToString();
            }

            if (StackTrace != null)
            {
                s += Environment.NewLine + StackTrace;
            }

            return s;
        }

        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("RequiredPlaceholderIsNull_ColumnName", _columnName, typeof(string));
        }
    }
}
