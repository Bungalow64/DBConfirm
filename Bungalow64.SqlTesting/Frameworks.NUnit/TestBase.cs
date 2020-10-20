using System.Threading.Tasks;
using Models.TestFrameworks.Abstract;
using NUnit.Framework;
using Models.Runners.Abstract;

namespace Frameworks.NUnit
{
    /// <summary>
    /// The abstract base class for test classes using NUnit
    /// </summary>
    public abstract class TestBase : BaseTestBase
    {
        /// <summary>
        /// Gets and sets the <see cref="ITestFramework"/> to be used for assertions, by default using <see cref="NUnitFramework"/> 
        /// </summary>
        protected override ITestFramework TestFramework { get; set; } = new NUnitFramework();

        /// <summary>
        /// Gets the value of the parameter from <see cref="TestContext"/>.  If the parameter does not exist, null is returned
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <returns>Returns the value of the parameter, or null if the parameter is not found</returns>
        protected override string GetParameter(string parameterName)
        {
            if (TestContext.Parameters.Exists(parameterName))
            {
                return TestContext.Parameters[parameterName];
            }
            return null;
        }

        /// <summary>
        /// The initialisation called before each test, to set up the <see cref="ITestRunner"/> for the test, and making the initial connection to the target database
        /// </summary>
        /// <returns>A task representing the asynchronous operation</returns>
        [SetUp]
        public Task Init() => BaseInit();

        /// <summary>
        /// The cleaup called after each test, to dispose the current instance of <see cref="ITestRunner"/>, rolling back the transaction and closing the connection
        /// </summary>
        [TearDown]
        public void Cleanup() => BaseCleanup();
    }
}
