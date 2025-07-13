using Microsoft.Extensions.Configuration;
using Sample.Core.XUnit.Tests.Common;
using Xunit;

namespace Sample.Core.XUnit.Tests;

public class CorrectnessTests
{
    [Fact]
    public void Correctness_VerifyConnectionString()
    {
        string connection = Initialisation.InitConfiguration().GetConnectionString("DefaultConnectionString");
        Assert.Equal("SERVER=localhost,1401;DATABASE=SampleDB;User Id=sa;Password=123qwe123qwe!;Connection Timeout=30;", connection);
    }
}
