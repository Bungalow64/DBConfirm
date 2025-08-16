using System;
using System.Threading.Tasks;
using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Core.Runners.Abstract;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace DBConfirm.Frameworks.XUnit
{
    /// <summary>
    /// The abstract base class for test classes using XUnit
    /// </summary>
    public abstract class XUnitFrameworkBase : BaseTestBase, IDisposable
    {
        /// <summary>
        /// Gets and sets the <see cref="ITestFramework"/> to be used for assertions, by default using <see cref="XUnitFramework"/> 
        /// </summary>
        protected override ITestFramework TestFramework { get; set; } = new XUnitFramework();

        /// <summary>
        /// Not available for XUnit
        /// </summary>
        /// <returns>null</returns>
        protected override string GetParameter(string parameterName)
        {
            return null;
        }

        /// <summary>
        /// The initialisation called before each test, to set up the <see cref="ITestRunner"/> for the test, and making the initial connection to the target database
        /// </summary>
        protected XUnitFrameworkBase(ITestRunnerFactory testRunnerFactory) : base(testRunnerFactory) => Task.Run(BaseInit).Wait();

        /// <summary>
        /// The cleanup called after each test, to dispose the current instance of <see cref="ITestRunner"/>, rolling back the transaction and closing the connection
        /// </summary>
        ~XUnitFrameworkBase() => BaseCleanup();

        /// <inheritdoc />
        public void Dispose()
        {
            BaseCleanup();
            GC.SuppressFinalize(this);
        }
    }
}
