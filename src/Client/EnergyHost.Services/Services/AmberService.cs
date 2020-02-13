using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
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
        private string _amberLoginUrl;
        private string _amberUsageUrl ;
        private string _amberUserName;
        private string _amberPassword;

        public AmberService(ILogService logService, IOptions<EnergyHostSettings> settings)
        {
            _logService = logService;
            _settings = settings;
            this._amberUrl = _settings.Value.AMBER_API_URL;
            _amberLoginUrl = _settings.Value.AMBER_LOGIN_URL;
            _amberUsageUrl = _settings.Value.AMBER_USAGE_URL;
            _amberPassword = _settings.Value.AMBER_PASSWORD;
            _amberUserName = _settings.Value.AMBER_USERNAME;
            
        }
        public double _inPrice(AmberData data, VariablePricesAndRenewable variables)
        {
            var price = ((data.data.staticPrices.E1.totalfixedKWHPrice + data.data.staticPrices.E1.lossFactor * variables.wholesaleKWHPrice) / 1.1) + 4 - 1.3;

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

        public async Task<AmberUsage> GetUsage()
        {
            var data = $"{{\"email\": \"{_amberUserName}\"}}";

            var login = await Login();


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("authorization", new List<string>()
                {
                    login.data.idToken
                });

                client.DefaultRequestHeaders.Add("refreshtoken", new List<string>()
                {
                    login.data.refreshToken
                });

                var result = await client.PostAsync(new Uri(_amberUsageUrl), new StringContent(data, Encoding.UTF8, "application/json"));

                if (!result.IsSuccessStatusCode)
                {
                    return null;
                }

                var stringResult = await result.Content.ReadAsStringAsync();
                var amberData = JsonConvert.DeserializeObject<AmberUsage>(stringResult, new JsonSerializerSettings
                {
                    Error = HandleDeserializationError
                });

                return amberData;
            }

           

            
        }



        public async Task<AmberLogin> Login()
        {
            var data = $"{{\"username\": \"{_amberUserName}\", password:\"{_amberPassword}\"}}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               

                var result = await client.PostAsync(new Uri(_amberLoginUrl), new StringContent(data, Encoding.UTF8, "application/json"));

                if (!result.IsSuccessStatusCode)
                {
                    return null;
                }

                var stringResult = await result.Content.ReadAsStringAsync();
                var amberData = JsonConvert.DeserializeObject<AmberLogin>(stringResult, new JsonSerializerSettings
                {
                    Error = HandleDeserializationError
                });

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


