namespace DBConfirm.Core.Parameters
{
    /// <summary>
    /// Defines a parameter to be used during SQL execution
    /// </summary>
    public class SqlQueryParameter
    {
        /// <summary>
        /// Gets or sets the name of the parameter
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// Gets or sets the parameter value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SqlQueryParameter() { }

        /// <summary>
        /// Constructor, with parameter name and value
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="value">The parameter value</param>
        public SqlQueryParameter(string parameterName, object value)
        {
            ParameterName = parameterName;
            Value = value;
        }
    }
}
