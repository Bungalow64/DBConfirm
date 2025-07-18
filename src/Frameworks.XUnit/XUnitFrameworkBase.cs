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
        /// Gets the value of the parameter from <see cref="TestContext"/>.  If the parameter does not exist, null is returned
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <returns>Returns the value of the parameter, or null if the parameter is not found</returns>
        protected override string GetParameter(string parameterName)
        {
            //if (TestContext.Current.Parameters.Exists(parameterName))
            //{
            //    return TestContext.Parameters[parameterName];
            //}
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
