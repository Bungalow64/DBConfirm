using SQLConfirm.Core.Data;

namespace SQLConfirm.Core.Templates.Abstract
{
    /// <summary>
    /// The interface for templates that relate to a single table
    /// </summary>
    public interface ISimpleTemplate : ITemplate
    {
        /// <summary>
        /// Gets the name of the table
        /// </summary>
        string TableName { get; }
        /// <summary>
        /// Gets the custom data set for the template
        /// </summary>
        DataSetRow CustomData { get; }
        /// <summary>
        /// Gets the default data set for the template
        /// </summary>
        DataSetRow DefaultData { get; }
        /// <summary>
        /// Gets the result of the default and custom data merged together
        /// </summary>
        DataSetRow MergedData { get; }
    }
}
