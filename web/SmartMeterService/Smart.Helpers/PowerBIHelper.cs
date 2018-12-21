using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Smart.Helpers
{
    public static class PowerBIHelper
    {
        public static async Task Push(double kwh, string url)
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
                maxvalue = max
            };

            var ser = JsonConvert.SerializeObject(model);
            Console.WriteLine($"Pushing: {ser}");

            var client = new HttpClient();

            HttpContent content = new StringContent(ser);
            HttpResponseMessage response = await client.PostAsync(url, content);
            //response.EnsureSuccessStatusCode();
        }
    }
}
