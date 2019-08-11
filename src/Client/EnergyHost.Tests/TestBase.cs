using System;
using System.Collections.Generic;
using System.Text;
using EnergyHost.Services.ServiceSetup;

namespace EnergyHost.Tests
{
    public class TestBase: AppHost<TestBase>
    {
        public TestBase()
        {
            Boot();
        }
    }
}
