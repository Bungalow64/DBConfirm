using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Models.Exceptions
{
    [Serializable]
    [ComVisible(true)]
    public class RequiredPlaceholderIsNullException : Exception
    {
        private readonly string _columnName;
        private const string _message = "The column is marked as required but has had no value set";

        public string ColumnName => _columnName;

        public RequiredPlaceholderIsNullException()
            : base(_message)
        {
        }

        public RequiredPlaceholderIsNullException(string message)
            : base(message)
        {
        }

        public RequiredPlaceholderIsNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RequiredPlaceholderIsNullException(string message, string columnName)
            : base(message)
        {
            _columnName = columnName;
        }

        public RequiredPlaceholderIsNullException(string message, string columnName, Exception innerException)
            : base(message, innerException)
        {
            _columnName = columnName;
        }

        protected RequiredPlaceholderIsNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _columnName = info.GetString("RequiredPlaceholderIsNull_ColumnName");
        }

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

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("RequiredPlaceholderIsNull_ColumnName", _columnName, typeof(string));
        }
    }
}
