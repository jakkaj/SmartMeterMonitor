using System;
using EnergyHost.Contract;

namespace EnergyHost.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var appHost = new AppHostBase();

            var appStartup = appHost.Resolve<IAppStartupService>();

            var result = appStartup.RunApp();

            int n;

            bool isNumeric = int.TryParse(result, out n);

            if (isNumeric)
            {
                Environment.Exit(n);
                return;
            }
            
            Console.WriteLine(result);
        }

        static void _setVar(string var, string value)
        {
            System.Environment.SetEnvironmentVariable($"PrdSettings__{var}", value);
        }
    }
}
