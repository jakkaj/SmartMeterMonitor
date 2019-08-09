using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.EnergyModels.Status.Base;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace EnergyHost.Services.Services
{
    public class MQTTService : IMQTTService
    {
        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;

        public event EventHandler<StatusEventArgs> EventReceived;
        private static DateTime _lastReading;
        public double KWH { get; set; }

        private IMqttClient _mqttClient = null;
        private IMqttClientOptions _options;

        public Dictionary<string,object> Values{get;set;} = new Dictionary<string, object>();

        public MQTTService(
            ILogService logService,
            IOptions<EnergyHostSettings> settings)
        {
            _logService = logService;
            _settings = settings;

            Values.Add("temp1", 0);
            Values.Add("humid1", 0);
        }

      

        public async Task Send(string topic = "events", string payload = null)
        {
            var count = 0;

            while (_mqttClient == null || !_mqttClient.IsConnected)
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                count++;
                if(count > 10)
                {
                    _logService.WriteError($"Could not send queue message with payload: {payload}");
                    return;
                }
            }

            var messagePayload = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build();

            try
            {
                await _mqttClient.PublishAsync(messagePayload);
            }
            catch (Exception ex)
            {
                _logService.WriteError(ex.ToString());
            }
        }

        public async Task Setup()
        {
            if (_mqttClient != null)
            {
                return;
            }

            var factory = new MqttFactory();

            _mqttClient = factory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                .WithTcpServer(_settings.Value.MQTT_SERVER_ADDRESS)
                .WithCleanSession()
                .Build();


            var lastMessageIn = DateTime.Now;

            _mqttClient.UseDisconnectedHandler(async e =>
            {
                _logService.WriteLog("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await _mqttClient.ConnectAsync(_options);
                }
                catch
                {
                    _logService.WriteError("### RECONNECTING FAILED ###");
                }
            });

            _mqttClient.UseConnectedHandler(async e =>
            {
                _logService.WriteLog("### CONNECTED WITH SERVER. Attempt subs ###");

                // Subscribe to a topic
                try
                {
                    await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("ctirms").Build());
                    await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("ctwatts").Build());
                    await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("temp1").Build());
                    await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("humid1").Build());
                    await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("pulsePeriod").Build());
                    await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("impressions").Build());
                    await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("log").Build());
                    await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("events").Build());
                }
                catch (Exception ex)
                {

                    _logService.WriteError(ex.Message);
                }

                _logService.WriteLog("### SUBSCRIBED ###");
            });

            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var value = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                _logService.WriteDebug($"Mqtt client received: ({topic}) {value}");

                if (topic == "temp1" || topic == "humid1")
                {
                    Values[topic] = Convert.ToDouble(value);
                    _logService.WriteDebug($"{topic}: {value}");
                }

                if(topic == "ctwatts" || topci == "ctirms"){
                    Values[topic] = Convert.ToDouble(value);
                    _logService.WriteDebug($"{topic}: {value}");
                }

                if (topic == "events")
                {
                    EventReceived?.Invoke(this, new StatusEventArgs() {Data = value});
                }

                if (topic == "pulsePeriod")
                {
                    _lastReading = DateTime.Now;
                    var val = Convert.ToInt32(value);
                    var kwh = _calcKWH(val);

                    _logService.WriteDebug($"KWH: {kwh}");

                    KWH = kwh;

                    lastMessageIn = DateTime.Now;
                }

            });

            await _mqttClient.ConnectAsync(_options);
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
