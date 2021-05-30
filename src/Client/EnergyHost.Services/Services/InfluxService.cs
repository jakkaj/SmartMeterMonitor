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
using DarkSky.Services;
using EnergyHost.Contract;
using EnergyHost.Model.DataModels;
using EnergyHost.Model.Settings;
using EnergyHost.Services.Utils;
using InfluxDB.Collector;
using InfluxDB.LineProtocol.Client;
using InfluxDB.LineProtocol.Payload;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EnergyHost.Services.Services
{
    public class InfluxService : IInfluxService
    {
        bool _created;

        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        Dictionary<string, ILineProtocolClient> _lineProtocolClients = new Dictionary<string, ILineProtocolClient>();

        public InfluxService(
            ILogService logService,
            IOptions<EnergyHostSettings> settings,
            IHttpClientFactory httpClientFactory
            )
        {
            _logService = logService;
            _settings = settings;
            _httpClientFactory = httpClientFactory;
        }


        ILineProtocolClient _getLineProtocolClient(string db)
        {
            lock (_lineProtocolClients)
            {
                if (!_lineProtocolClients.ContainsKey(db))
                {
                    _lineProtocolClients.Add(db, new LineProtocolClient(new Uri(InfluxServerUrl), db));
                }
            }

            return _lineProtocolClients[db];
        }

        public async Task<InfluxResult> Query(string db, string query)
        {
            var url = $"{InfluxServerUrl}/query?db={db}&q={query}";
            using (var client = _httpClientFactory.CreateClient())
            {
                var result = await client.GetAsync(new Uri(url));
                if (!result.IsSuccessStatusCode)
                {
                    _logService.WriteError($"{result.StatusCode}, {result.ReasonPhrase}");
                    return null;
                }

                try
                {
                    var des = JsonConvert.DeserializeObject<InfluxResult>(await result.Content.ReadAsStringAsync());
                    return des;
                }
                catch (Exception ex)
                {
                    _logService.WriteError(ex);
                }
            }

            return null;

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
                //ts.Add("timestamp", utcTimeStamp?.ConvertToISO());
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

            var client = _getLineProtocolClient(db);

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

            await Write(db, measurement, data, null, null);
        }

        private async Task _createDatabase(string influxDbUrl, string dbName)
        {
            if (_created)
            {
                return;
            }
            try
            {
                var url = new Uri($"{influxDbUrl}/query?q=CREATE DATABASE {dbName}");
                var client = _httpClientFactory.CreateClient();
                await client.GetAsync(url);
                _created = true;
            }
            catch (Exception ex)
            {
                _logService.WriteError(ex.ToString());
            }
            
        }
    }
}
