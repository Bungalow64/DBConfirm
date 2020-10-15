using Models.Factories.Abstract;
using Frameworks.MSTest2.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Abstract;
using Moq;
using System.Threading.Tasks;
using Models.TestFrameworks.Abstract;

namespace Frameworks.MSTest2.Tests
{
    [TestClass]
    public class TestBaseTests
    {
        #region Setup

        private Mock<ITestRunnerFactory> _testRunnerFactoryMock;
        private Mock<ITestRunner> _testRunnerMock;

        [TestInitialize]
        public void Init()
        {
            _testRunnerFactoryMock = new Mock<ITestRunnerFactory>(MockBehavior.Strict);
            _testRunnerMock = new Mock<ITestRunner>(MockBehavior.Strict);

        }

        private MockedTestClass GetTestClass()
        {
            MockedTestClass testClass = new MockedTestClass
            {
                TestRunnerFactory = _testRunnerFactoryMock.Object
            };
            return testClass;
        }

        #endregion

        [TestMethod]
        public async Task TestBase_Init_InitialiseAsyncCalledCorrectly()
        {
            _testRunnerFactoryMock
                .Setup(p => p.BuildTestRunner(It.IsAny<string>()))
                .Callback<string>(p => Assert.AreEqual("SERVER=(local);DATABASE=SampleDB;Integrated Security=true;Connection Timeout=30;", p))
                .Returns(_testRunnerMock.Object);

            _testRunnerMock
                .Setup(p => p.InitialiseAsync(It.IsAny<ITestFramework>()))
                .Returns(Task.CompletedTask);

            await GetTestClass().Init();

            _testRunnerFactoryMock
                .Verify(p => p.BuildTestRunner(It.IsAny<string>()), Times.Once);

            _testRunnerMock
                .Verify(p => p.InitialiseAsync(It.IsAny<ITestFramework>()), Times.Once);
        }

        [TestMethod]
        public async Task TestBase_Dispose_DisposeCalledCorrectly()
        {
            _testRunnerFactoryMock
                .Setup(p => p.BuildTestRunner(It.IsAny<string>()))
                .Returns(_testRunnerMock.Object);

            _testRunnerMock
                .Setup(p => p.InitialiseAsync(It.IsAny<ITestFramework>()))
                .Returns(Task.CompletedTask);

            _testRunnerMock
                .Setup(p => p.Dispose());

            var testClass = GetTestClass();

            await testClass.Init();

            testClass.Cleanup();

            _testRunnerMock
                .Verify(p => p.Dispose(), Times.Once);
        }
    }
}
