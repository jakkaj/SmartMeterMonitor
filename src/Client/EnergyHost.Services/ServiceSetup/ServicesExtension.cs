using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnergyHost.Services.ServiceSetup
{
    public static class ServicesExtension
    {
        public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            new ServiceHost().Configure(services, configuration);
        }
    }
}