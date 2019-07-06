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

            var lastMessageIn = DateTime.Now;

            mqttClient.Disconnected += async (s, e) =>
            {
                _logService.WriteLog("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await mqttClient.ConnectAsync(options);
                }
                catch
                {
                    _logService.WriteError("### RECONNECTING FAILED ###");
                }
            };

            mqttClient.Connected += async (s, e) =>
            {
                _logService.WriteLog("### CONNECTED WITH SERVER. Attempt subs ###");

                // Subscribe to a topic
                try
                {
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("pulsePeriod").Build());
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("impressions").Build());
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("log").Build());
                }
                catch (Exception ex)
                {

                    _logService.WriteError(ex.Message);
                }

                _logService.WriteLog("### SUBSCRIBED ###");
            };

            mqttClient.ApplicationMessageReceived += async (s, e) =>
            {
                var topic = e.ApplicationMessage.Topic;
                var value = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                _logService.WriteDebug($"Mqtt client received: ({topic}) {value}");

                if (topic == "pulsePeriod")
                {
                    _lastReading = DateTime.Now;
                    var val = Convert.ToInt32(value);
                    var kwh = _calcKWH(val);

                    _logService.WriteDebug($"KWH: {kwh}");

                    KWH = kwh;

                    MessageReceived?.Invoke(this, EventArgs.Empty);
                    lastMessageIn = DateTime.Now;
                }

            };

            await mqttClient.ConnectAsync(options);
#pragma warning disable 4014
            Task.Run(async () =>
#pragma warning restore 4014
            {
                while (true)
                {
                    if (DateTime.Now.Subtract(lastMessageIn) > TimeSpan.FromSeconds(20))
                    {
                        KWH = 0;
                    }

                    await Task.Delay(2000);
                }
            });


        }

        private const int impKwh = 1000;

        public double _calcKWH(int val)
        {
            double msToKwh = val * impKwh;
            double secToKwh = msToKwh / 1000;
            return 3600 / secToKwh;
        }

    }
}
