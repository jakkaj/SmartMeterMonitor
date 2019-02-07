
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MqttTestClient
{
    public class InfluxHelper
    {
        public static async Task CreateDatabase(string influxDbUrl)
        {
            var url = new Uri($"{influxDbUrl}/query?q=CREATE DATABASE kwh");
            var c = new HttpClient();
            await c.GetAsync(url);
        }
    }
}