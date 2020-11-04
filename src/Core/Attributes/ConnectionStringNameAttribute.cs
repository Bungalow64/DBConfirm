using System;

namespace DBConfirm.Core.Attributes
{
    /// <summary>
    /// Configures the connection string name to use for all tests within the class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ConnectionStringNameAttribute : Attribute
    {
        /// <summary>
        /// Contructor, setting the connection string name to use for all tests within the class
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string to use</param>
        public ConnectionStringNameAttribute(string connectionStringName)
        {
            ConnectionStringName = connectionStringName;
        }

        /// <summary>
        /// Gets the name of the connection string to use for all tests within the class
        /// </summary>
        public string ConnectionStringName { get; }
    }
}
