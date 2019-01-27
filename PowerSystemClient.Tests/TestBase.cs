using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PowerSystemClient.Tests
{
    public class TestBase
    {
        public IConfiguration Config { get; }
        public TestBase()
        {
            Config = _getConfig();
        }
        IConfiguration _getConfig()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<TestBase>()
                .AddJsonFile($"appsettings.json", true, true)
               
                .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}
