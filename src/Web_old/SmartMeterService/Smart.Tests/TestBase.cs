using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Configuration.Json;
using Smart.Helpers;
using Smart.Helpers.Contracts;
using Smart.Helpers.DB;
using Smart.Helpers.Service;

namespace Smart.Tests
{
    public class TestBase
    {
        public static IOptions<PowerSecrets> SecretOptions;
        
        public static ServiceProvider Services { get; set; }
        static TestBase()
        {
            var builder = new ConfigurationBuilder();
            // tell the builder to look for the appsettings.json file
            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
         
            builder.AddUserSecrets<TestBase>();

            var configuration = builder.Build();

            var connection = configuration.GetSection("PowerSecrets").Get<PowerSecrets>().DBConnectionString;

            var services = new ServiceCollection();
            

            var serviceProvider = services
                .Configure<PowerSecrets>(configuration.GetSection(nameof(PowerSecrets)))
                .AddSingleton<IDatabaseService, DatabaseService>()
                .AddDbContext<PowerContext>
                    (options => options.UseSqlServer(connection))
                .AddOptions()
                .BuildServiceProvider();

            Services = serviceProvider;

            var opts = serviceProvider.GetService<IOptions<PowerSecrets>>();

            SecretOptions = opts;

            

        }

        public static T Resolve<T>()
        {
            return Services.GetService<T>();
        }
    }
}
