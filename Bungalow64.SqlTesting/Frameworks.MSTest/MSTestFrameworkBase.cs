using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLConfirm.Core.TestFrameworks.Abstract;
using SQLConfirm.Core.Runners.Abstract;

namespace SQLConfirm.Frameworks.MSTest
{
    /// <summary>
    /// The abstract base class for test classes using MSTest
    /// </summary>
    public abstract class MSTestFrameworkBase : BaseTestBase
    {
        /// <summary>
        /// The current <see cref="TestContext"/> instance
        /// </summary>
        protected static TestContext Context { get; set; }

        /// <summary>
        /// Gets and sets the <see cref="ITestFramework"/> to be used for assertions, by default using <see cref="MSTestFramework"/> 
        /// </summary>
        protected override ITestFramework TestFramework { get; set; } = new MSTestFramework();

        /// <summary>
        /// The initialisation called once before all tests are run in a class, to set the current <see cref="TestContext"/> object
        /// </summary>
        /// <param name="testContext">The current <see cref="TestContext"/> instance</param>
        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassInitialise(TestContext testContext)
        {
            Context = testContext;
        }

        /// <summary>
        /// Gets the value of the parameter from the current <see cref="TestContext"/> instance.  If the parameter does not exist, null is returned
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <returns>Returns the value of the parameter, or null if the parameter is not found</returns>
        protected override string GetParameter(string parameterName)
        {
            if (Context != null && Context.Properties.Contains(parameterName))
            {
                return Context.Properties[parameterName]?.ToString();
            }
            return null;
        }

        /// <summary>
        /// The initialisation called before each test, to set up the <see cref="ITestRunner"/> for the test, and making the initial connection to the target database
        /// </summary>
        /// <returns>A task representing the asynchronous operation</returns>
        [TestInitialize]
        public Task Init() => BaseInit();

        /// <summary>
        /// The cleaup called after each test, to dispose the current instance of <see cref="ITestRunner"/>, rolling back the transaction and closing the connection
        /// </summary>
        [TestCleanup]
        public void Cleanup() => BaseCleanup();
    }
}
