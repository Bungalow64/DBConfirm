using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace DBConfirm.Databases.MySQL.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a table cannot be found
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class TableNotFoundException : Exception
    {
        private readonly string _tableName;
        private const string _message = "The table cannot be found";
        private const string _columnTablePlaceholderMessage = "The table {0} cannot be found";

        /// <summary>
        /// The name of the missing table
        /// </summary>
        public string TableName => _tableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableNotFoundException"/> class
        /// </summary>
        public TableNotFoundException()
            : base(_message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableNotFoundException"/> class with the name of the missing table
        /// </summary>
        /// <param name="tableName">The name of the table missing a value</param>
        public TableNotFoundException(string tableName)
            : base(string.Format(_columnTablePlaceholderMessage, tableName ?? string.Empty))
        {
            _tableName = tableName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableNotFoundException"/> class with a specified error message and the exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public TableNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableNotFoundException"/> class with serialized data
        /// </summary>
        /// <param name="info">The object that holds the serialized object data</param>
        /// <param name="context">An object that describes the source or destination of the serialized data</param>
        protected TableNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _tableName = info.GetString("TableNotFound_TableName");
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            string s = GetType().FullName + ": " + Message;

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

            info.AddValue("TableNotFound_TableName", _tableName, typeof(string));
        }
    }
}
