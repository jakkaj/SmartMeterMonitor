using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using EnergyHost.Services.Utils;
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

        public async Task<bool> WriteObject(string db, string measurement, object data,
            IReadOnlyDictionary<string, string> tags = null, DateTime? utcTimeStamp = null)
        {
            var dict = data.ConvertToDictionary();
            return await Write(db, measurement, dict, tags, utcTimeStamp);
        }

        public async Task<bool> Write(string db, string measurement, IReadOnlyDictionary<string, object> data, IReadOnlyDictionary<string, string> tags = null, DateTime? utcTimeStamp = null)
        {
            
            await _createDatabase(InfluxServerUrl, db);

            var ts = new Dictionary<string, string>();

//remember to check the container is runnings in the right timzone!
            if (utcTimeStamp != null)
            {                
                ts.Add("timestamp", utcTimeStamp?.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
            }
            else
            {
                utcTimeStamp = DateTime.UtcNow;
            }

            if (tags != null)
            {
                foreach (var key in tags.Keys)
                {
                    ts.Add(key, tags[key]);
                }
            }

            var writer = new LineProtocolPoint(
                measurement,
               data,
                ts,
                utcTimeStamp);

            var payload = new LineProtocolPayload();
            payload.Add(writer);

            var client = new LineProtocolClient(new Uri(InfluxServerUrl), db);

            try
            {
                var influxResult = await client.WriteAsync(payload);



                if (!influxResult.Success)
                    _logService.WriteError(influxResult.ErrorMessage);

                return influxResult.Success;
            }
            catch (Exception ex)
            {
                _logService.WriteError(ex.ToString());
                return false;
            }
            
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
            try
            {
                var url = new Uri($"{influxDbUrl}/query?q=CREATE DATABASE {dbName}");
                var c = new HttpClient();
                await c.GetAsync(url);
            }
            catch (Exception ex)
            {
                _logService.WriteError(ex.ToString());
            }
            
        }
    }
}
