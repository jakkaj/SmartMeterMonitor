using System;
using System.Collections.Generic;
using System.Text;
using EnergyHost.Services.ServiceSetup;

namespace EnergyHost.App
{
    public class AppHostBase : AppHost<AppHostBase>
    {
        /// <summary>
        /// </summary>
        public AppHostBase()
        {
            Boot();
        }
    }
}
