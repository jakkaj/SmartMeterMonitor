using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;
namespace PowerSystemClient.Tests
{
    public class TestBase
    {
        public IConfiguration Config { get; }
        public TestBase()
        {
            Config = _getConfig();
            var file = File.Open("/tmp/log.txt", FileMode.OpenOrCreate);

            Trace.Listeners.Add(new TextWriterTraceListener(file));
            
            //Trace.Listeners.Add(new FileWr)
        }

        public void WriteTrace(string message){    
                    
            Trace.WriteLine(message);
            Trace.Flush();
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
