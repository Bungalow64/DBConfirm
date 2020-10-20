using Models.Runners.Abstract;
using System.Threading.Tasks;

namespace Models.Templates.Abstract
{
    /// <summary>
    /// The interface for a template, used to set up precondition data in the target database
    /// </summary>
    public interface ITemplate
    {
        /// <summary>
        /// Gets whether the template has been executed and inserted into the target database
        /// </summary>
        bool IsInserted { get; }
        /// <summary>
        /// Records that the template has been executed and inserted into the target database
        /// </summary>
        void RecordInsertion();
        /// <summary>
        /// Inserts the template data into the target database
        /// </summary>
        /// <param name="testRunner">The test runner used for the connection to the target database</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task InsertAsync(ITestRunner testRunner);
    }
}
