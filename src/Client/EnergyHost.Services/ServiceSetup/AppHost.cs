using System;
using EnergyHost.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnergyHost.Services.ServiceSetup
{
    public class AppHost<THostAppClass> where THostAppClass : class
    {
        private IServiceProvider ServiceProvider { get; set; }
        private IServiceCollection ServiceCollection { get; set; }

        public void Boot()
        {
            try
            {
                DotNetEnv.Env.Load(true, true, true, false);

            }
            catch
            {
                //ignore, the file was missing
            }


            var builder = new ConfigurationBuilder();
            // tell the builder to look for the appsettings.json file
            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            //builder.AddUserSecrets<THostAppClass>();

            var configuration = builder.Build();


            ServiceCollection = new ServiceCollection();

            ServiceCollection.AddCoreServices(configuration);


            ServiceProvider = ServiceCollection.BuildServiceProvider();

            _bootAlerts();
        }

        void _bootAlerts()
        {
            var time = Resolve<ITimeAlertService>(); //just resolve and IOC container will hold on to it. 
        }

        public T Resolve<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}