using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using InfluxDB.Collector;
using Microsoft.Extensions.Options;

namespace EnergyHost.Services.Services
{
    public class InfluxService : IInfluxService
    {
        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;

        public InfluxService(
            ILogService logService,
            IOptions<EnergyHostSettings> settings
            )
        {
            _logService = logService;
            _settings = settings;
        }
        public async Task Write(string db, string measurement, Dictionary<string, object> data)
        {
            var influxUrl = $"http://{_settings.Value.INFLUX_SERVER_ADDRESS}:8086";

            await _createDatabase(influxUrl, db);


            Metrics.Collector = new CollectorConfiguration()
                
                .Tag.With("host", "campbellst")
                .WriteTo.InfluxDB(influxUrl, db)
                .CreateCollector();


            Metrics.Write(measurement, data);
        }

        private async Task _createDatabase(string influxDbUrl, string dbName)
        {
            var url = new Uri($"{influxDbUrl}/query?q=CREATE DATABASE {dbName}");
            var c = new HttpClient();
            await c.GetAsync(url);
        }
    }
}
