using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;

namespace EnergyHost.Services.Services
{
    public class MQTTService : IMQTTService
    {
        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;

        public event EventHandler MessageReceived;
        private static DateTime _lastReading;
        public double KWH { get; set; }

        public MQTTService(
            ILogService logService,
            IOptions<EnergyHostSettings> settings)
        {
            _logService = logService;
            _settings = settings;
        }


        public async Task Setup()
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(_settings.Value.MQTT_SERVER_ADDRESS)
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
                }
                catch (Exception ex)
                {
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
                    var kwh = _calcKWH(val);

                    KWH = kwh;

                    MessageReceived?.Invoke(this, EventArgs.Empty);
                }

            };

            await mqttClient.ConnectAsync(options);

        }

        private const int impKwh = 800;

        public double _calcKWH(int val)
        {

            Console.WriteLine(val);
            double msToKwh = val * impKwh;
            double secToKwh = msToKwh / 1000;
            return 3600 / secToKwh;
        }

    }
}
