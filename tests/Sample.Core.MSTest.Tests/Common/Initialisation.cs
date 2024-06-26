﻿using Microsoft.Extensions.Configuration;

namespace Sample.Core.MSTest.Tests.Common;

public class Initialisation
{
    public static IConfiguration InitConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }
}
