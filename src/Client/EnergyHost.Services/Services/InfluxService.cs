using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using InfluxDB.Collector;
using InfluxDB.LineProtocol.Client;
using InfluxDB.LineProtocol.Payload;
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

        private string InfluxServerUrl => $"http://{_settings.Value.INFLUX_SERVER_ADDRESS}:8086";

        public async Task<bool> Write(string db, string measurement, IReadOnlyDictionary<string, object> data, IReadOnlyDictionary<string, string> tags = null, DateTime? utcTimeStamp = null)
        {
            await _createDatabase(InfluxServerUrl, db);

            if (utcTimeStamp == null)
            {
                utcTimeStamp = DateTime.UtcNow;
            }

            var writer = new LineProtocolPoint(
                measurement,
               data,
                tags,
                utcTimeStamp);

            var payload = new LineProtocolPayload();
            payload.Add(writer);

            var client = new LineProtocolClient(new Uri(InfluxServerUrl), db);
            var influxResult = await client.WriteAsync(payload);

        

            if (!influxResult.Success)
                _logService.WriteError(influxResult.ErrorMessage);

            return influxResult.Success;
        }
        public async Task Write(string db, string measurement, Dictionary<string, object> data)
        {

            await _createDatabase(InfluxServerUrl, db);


            Metrics.Collector = new CollectorConfiguration()
                
                .Tag.With("host", "campbellst")
                .WriteTo.InfluxDB(InfluxServerUrl, db)
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
