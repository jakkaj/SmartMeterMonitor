﻿using System;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using EnergyHost.Services.Contract;
using EnergyHost.Services.Services;
using EnergyHost.Services.Services.AlertServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnergyHost.Services.ServiceSetup
{
    public class ServiceHost : IServiceHost
    {
        public IServiceCollection Services { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }


        public ServiceHost Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EnergyHostSettings>(configuration.GetSection(nameof(EnergyHostSettings)));
            services.AddTransient<IAppStartupService, AppStartupService>();
            services.AddTransient<IAmberService, AmberService>();
            services.AddTransient<IDarkSkyService, DarkSkyService>();
            services.AddTransient<IEnergyFuturesService, EnergyFuturesService>();
            services.AddTransient<IInfluxService, InfluxService>();
            
            services.AddSingleton<IMQTTService, MQTTService>();
            services.AddTransient<IDataLoggerService, DataLoggerService>();
            services.AddTransient<IDaikinService, DaikinService>();
            services.AddTransient<IABBService, ABBService>();
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<ISystemStatusService, SystemStatusService>();
            services.AddSingleton<ITimeAlertService, TimeAlertService>();
            services.AddSingleton<IThresholdingService, ThresholdingService>();
            services.AddSingleton<INotificationService, NotificationService>();

            services.AddHttpClient();

            return this;
        }

        public IServiceProvider Build()
        {
            ServiceProvider = Services.BuildServiceProvider();
            return ServiceProvider;
        }
    }
}