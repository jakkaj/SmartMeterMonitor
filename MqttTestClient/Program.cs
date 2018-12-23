﻿using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Adapter;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Exceptions;
using MQTTnet.Packets;
using MQTTnet.Server;
using MQTTnet;
using Smart.Helpers;
namespace MqttTestClient
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
            .WithTcpServer("10.0.0.59")
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
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("pulsePeriod").Build());
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("impressions").Build());
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("log").Build());


                //Console.WriteLine("### SUBSCRIBED ###");
            };

            await mqttClient.ConnectAsync(options);

            mqttClient.ApplicationMessageReceived += (s, e) =>
            {
                var topic = e.ApplicationMessage.Topic;
                var value = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"({topic}) {value}");
                
                if(topic == "pulsePeriod"){
                    var val = Convert.ToInt32(value);
                    var kwh = KWHelper.CalcKWH(1, val / 1000);
                    Console.WriteLine($"kwh -> {kwh.ToString("0.##")}");
                }
                
                // Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                // Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                // Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                // Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                // Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                // Console.WriteLine();
            };
            Console.ReadLine();
        }
    }
}
