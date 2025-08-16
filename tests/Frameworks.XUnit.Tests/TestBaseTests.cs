using System;
using System.Threading.Tasks;
using DBConfirm.Core.Factories.Abstract;
using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using Frameworks.XUnit.Tests.TestHelpers;
using Moq;
using Xunit;

namespace Frameworks.XUnit.Tests;

public class TestBaseTests
{
    #region Setup

    private readonly Mock<ITestRunnerFactory> _testRunnerFactoryMock = new(MockBehavior.Strict);
    private readonly Mock<ITestRunner> _testRunnerMock = new(MockBehavior.Strict);

    private MockedTestClass GetTestClass()
    {
        MockedTestClass testClass = new(_testRunnerFactoryMock.Object);
        return testClass;
    }

    #endregion

    [Fact]
    public async Task TestBase_Init_InitialiseAsyncCalledCorrectly()
    {
        _testRunnerFactoryMock
            .Setup(p => p.BuildTestRunner(It.IsAny<string>()))
            .Callback<string>(p => Assert.Equal("SERVER=(local);DATABASE=SampleDB;Integrated Security=true;Connection Timeout=30;", p))
            .Returns(_testRunnerMock.Object);

        _testRunnerMock
            .Setup(p => p.InitialiseAsync(It.IsAny<ITestFramework>()))
            .Returns(Task.CompletedTask);

        GetTestClass();

        _testRunnerFactoryMock
            .Verify(p => p.BuildTestRunner(It.IsAny<string>()), Times.Once);

        _testRunnerMock
            .Verify(p => p.InitialiseAsync(It.IsAny<ITestFramework>()), Times.Once);
    }

    [Fact]
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

        using (var testClass = GetTestClass())
        {

        }

        GC.Collect(2);

        _testRunnerMock
            .Verify(p => p.Dispose(), Times.Once);
    }
}
