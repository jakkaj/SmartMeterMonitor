using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AmberElectric.Model;
using Newtonsoft.Json;

namespace AmberElectric.Service
{
    public class AmberService
    {
        public double InPrice(AmberData data, VariablePricesAndRenewable variables)
        {
            var price = (data.data.staticPrices.E1.totalfixedKWHPrice + data.data.staticPrices.E1.lossFactor * data.data.variablePricesAndRenewables[0].wholesaleKWHPrice) / 1.1;

            return price;
        }

        public double OutPrice(AmberData data, VariablePricesAndRenewable variables)
        {
            var price = (data.data.staticPrices.B1.totalfixedKWHPrice + data.data.staticPrices.B1.lossFactor * data.data.variablePricesAndRenewables[0].wholesaleKWHPrice) / 1.1;

            return price;
        }

        public async Task<AmberData> Get(string postCode)
        {
            var data = $"{{ \"postcode\": \"{postCode}\" }}";

            var url = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.PostAsync(new Uri(url), new StringContent(data));

                if (!result.IsSuccessStatusCode)
                {
                    return null;
                }

                var stringResult = await result.Content.ReadAsStringAsync();
                var deSerialise = JsonConvert.DeserializeObject<AmberData>(stringResult);
               
                
                var outPrice = (deSerialise.data.staticPrices.B1.totalfixedKWHPrice + deSerialise.data.staticPrices.B1.lossFactor * deSerialise.data.variablePricesAndRenewables[0].wholesaleKWHPrice) / 1.1;

                return deSerialise;
                
                
            }
        }
    }
}
