namespace SQLConfirm.Core.Templates.Abstract
{
    /// <summary>
    /// The interface for a deferred action
    /// </summary>
    public interface IResolver
    {
        /// <summary>
        /// Resolves the action to return the resulting object
        /// </summary>
        /// <returns>Returns the resulting object</returns>
        object Resolve();
    }
}
