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
            var model = new PowerBIModel
            {
                kwh = kwh,
                measuretime = DateTime.Now.ToString(timeFormat),
                kwhday = kwh * 24
            };

            var ser = JsonConvert.SerializeObject(model);

            var client = new HttpClient();

            HttpContent content = new StringContent(ser);
            HttpResponseMessage response = await client.PostAsync(url, content);
            //response.EnsureSuccessStatusCode();
        }
    }
}
