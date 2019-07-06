using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.EnergyModels;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EnergyHost.Services.Services
{
    public class AmberService : IAmberService
    {
        private readonly IOptions<EnergyHostSettings> _settings;
        private ILogService _logService;


        private string _amberUrl;

        public AmberService(ILogService logService, IOptions<EnergyHostSettings> settings)
        {
            _logService = logService;
            _settings = settings;
            this._amberUrl = _settings.Value.AMBER_API_URL;
        }
        public double _inPrice(AmberData data, VariablePricesAndRenewable variables)
        {
            var price = (data.data.staticPrices.E1.totalfixedKWHPrice + data.data.staticPrices.E1.lossFactor * variables.wholesaleKWHPrice) / 1.1;

            return price;
        }

        public double _outPrice(AmberData data, VariablePricesAndRenewable variables)
        {
            var price = (data.data.staticPrices.B1.totalfixedKWHPrice + data.data.staticPrices.B1.lossFactor * variables.wholesaleKWHPrice) / 1.1;

            return price;
        }

        public async Task<AmberData> Get(string postCode)
        {
            var data = $"{{ \"postcode\": \"{postCode}\" }}";

            var url = _amberUrl;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.PostAsync(new Uri(url), new StringContent(data));

                if (!result.IsSuccessStatusCode)
                {
                    return null;
                }

                var stringResult = await result.Content.ReadAsStringAsync();
                var amberData = JsonConvert.DeserializeObject<AmberData>(stringResult, new JsonSerializerSettings
                {
                    Error = HandleDeserializationError
                });




                foreach (var varPrice in amberData.data.variablePricesAndRenewables)
                {
                    varPrice.InPrice = _inPrice(amberData, varPrice);
                    varPrice.OutPrice = _outPrice(amberData, varPrice);
                }

                var maxIn = amberData.data.variablePricesAndRenewables.OrderByDescending(_ => _.InPrice).First().InPrice;
                var minIn = amberData.data.variablePricesAndRenewables.OrderBy(_ => _.InPrice).First().InPrice;
                var rangeIn = (double)(maxIn - minIn);

                var maxOut = amberData.data.variablePricesAndRenewables.OrderByDescending(_ => _.OutPrice).First().InPrice;
                var minOut = amberData.data.variablePricesAndRenewables.OrderBy(_ => _.OutPrice).First().InPrice;
                var rangeOut = (double)(maxOut - minOut);


                foreach (var varPrice in amberData.data.variablePricesAndRenewables)
                {
                    varPrice.InPriceNormal = (100 * (varPrice.InPrice - minIn) / rangeIn) / 100;
                    varPrice.OutPriceNormal = (100 * (varPrice.OutPrice - minOut) / rangeOut) / 100;
                }



                return amberData;


            }
        }

        public void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
            _logService.WriteError(currentError);
        }
    }
}


