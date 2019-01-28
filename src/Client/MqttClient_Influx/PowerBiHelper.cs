using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MqttTestClient
{
    public static class PowerBIHelper
    {
        public static async Task Push(string text, string url)
        {
            const string timeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ"; // Time format required by Power BI

            var model = new PowerBIModel
            {
                StatusText = text,
                measuretime = DateTime.UtcNow.ToString(timeFormat),
            };

            await PushToPowerBi(model, url);
        }

        public static async Task Push(double kwh, decimal rDollars, string url)
        {
            const string timeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ"; // Time format required by Power BI

            var max = (int)Math.Ceiling(kwh);
            if (max < 4)
            {
                max = 4;
            }

            var model = new PowerBIModel
            {
                kwh = kwh,
                measuretime = DateTime.UtcNow.ToString(timeFormat),
                kwhday = kwh * 24,
                maxvalue = max,
                CurrentDollars = rDollars
            };

            await PushToPowerBi(model, url);
        }

        static async Task PushToPowerBi(PowerBIModel model, string url)
        {
            var ser = JsonConvert.SerializeObject(model);
            Console.WriteLine($"Pushing: {ser}");

            var client = new HttpClient();

            HttpContent content = new StringContent(ser);
            HttpResponseMessage response = await client.PostAsync(url, content);
        }
    }
}
