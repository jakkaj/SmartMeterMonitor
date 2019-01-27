using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet.Adapter;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Exceptions;
using MQTTnet.Packets;
using MQTTnet.Server;
using MQTTnet;
using Smart.Helpers;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Schema;
using InfluxDB.Collector;
using InfluxDB.LineProtocol.Client;
using Microsoft.Extensions.Configuration;

namespace MqttTestClient
{
    class Program
    {
        private static double _kwh = 0;
        private static string _mqttServerAddress;
        private static string _influxServerAddress;
        private static string _powerBiServerUrl;

        private static DateTime _lastReading;

        public static async Task Main(string[] args)
        {
            _lastReading = DateTime.Now;
            var config = _getConfig();

            _mqttServerAddress = config["MQTT_SERVER_ADDRESS"];
            _influxServerAddress = config["INFLUX_SERVER_ADDRESS"];
            _powerBiServerUrl = config["POWER_BI_URL"];

            if (string.IsNullOrWhiteSpace(_mqttServerAddress) || string.IsNullOrWhiteSpace(_influxServerAddress)
                                                             || string.IsNullOrWhiteSpace(_powerBiServerUrl))
            {
                Console.WriteLine("Missing environment: MQTT_SERVER_ADDRESS or INFLUX_SERVER_ADDRESS or POWER_BI_URL");
                Environment.Exit(1);
                return;
            }


            Console.WriteLine($"Server boots. MQTT Server: {_mqttServerAddress}, Influx: {_influxServerAddress}, PowerBi: {_powerBiServerUrl}");

            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
            .WithTcpServer(_mqttServerAddress)
            .WithCleanSession()
            .Build();

            mqttClient.Disconnected += async (s, e) =>
            {
                Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await mqttClient.ConnectAsync(options);
                }
                catch
                {
                    Console.WriteLine("### RECONNECTING FAILED ###");
                }
            };

            mqttClient.Connected += async (s, e) =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER. Attempt subs ###");

                // Subscribe to a topic
                try
                {
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("pulsePeriod").Build());
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("impressions").Build());
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("log").Build());
                }catch(Exception ex){
                    Console.WriteLine("Exception");
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("### SUBSCRIBED ###");
            };

            

            mqttClient.ApplicationMessageReceived += async (s, e) =>
            {
                var topic = e.ApplicationMessage.Topic;
                var value = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"Mqtt client received: ({topic}) {value}");

                if (topic == "pulsePeriod")
                {
                    _lastReading = DateTime.Now;
                    var val = Convert.ToInt32(value);
                    var kwh = KWHelper.CalcKWH(val);

                    _kwh = kwh;

                    Console.WriteLine($"reading kwh -> {_kwh.ToString("0.##")}");
                }

            };

            await mqttClient.ConnectAsync(options);


            await _loop();

            //System.Threading.Thread.Sleep(Timeout.Infinite);
        }

        static IConfiguration _getConfig()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddEnvironmentVariables();
            return builder.Build();
        }

        static async Task _loop()
        {
            
            var influxUrl = $"http://{_influxServerAddress}:8086";

            await InfluxHelper.CreateDatabase(influxUrl);

            Metrics.Collector = new CollectorConfiguration()
                .Tag.With("host", "campbellst")
                .WriteTo.InfluxDB(influxUrl, "kwh")
                .CreateCollector();

            while (true)
            {
                if(DateTime.Now.Subtract(TimeSpan.FromSeconds(60)) > _lastReading){
                    _kwh = 0;
                    Console.WriteLine("Reading is stale");
                }
                Thread.Sleep(TimeSpan.FromSeconds(5));
                var resultDay = _kwh * 24;
                if (resultDay < 0)
                {
                    resultDay = 0;
                }

                var resultDollars = ((decimal)resultDay * 28.52M) / 100M;

                await PowerBIHelper.Push(_kwh, resultDollars, _powerBiServerUrl);
                Console.WriteLine("Sending");
                //var c = new HttpClient();
                Metrics.Write("kwh",
                    new Dictionary<string, object>
                    {
                        { "reading", _kwh }
                    });
            }
        }
    }
}
