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
        public static async Task Push(double kwh)
        {
            const string timeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ"; // Time format required by Power BI
            var model = new PowerBIModel
            {
                kwh = kwh,
                measuretime = DateTime.Now.ToString(timeFormat),
                kwhday = kwh * 24
            };

            var ser = JsonConvert.SerializeObject(model);

            var url =
                "https://api.powerbi.com/beta/72f988bf-86f1-41af-91ab-2d7cd011db47/datasets/1bcf75b6-70d1-4aee-bc4a-59754afd4122/rows?key=sfpIwaXpYRX1n%2ByUP%2BI526Sx2DGLafIQv4QbLP9K8UkLSoaTOA126OheI8NM0TI9GJy1OOordwjyrbDWoYP9xg%3D%3D";

            var client = new HttpClient();

            HttpContent content = new StringContent(ser);
            HttpResponseMessage response = await client.PostAsync(url, content);
            //response.EnsureSuccessStatusCode();
        }
    }
}
