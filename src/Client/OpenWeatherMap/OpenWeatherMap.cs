using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenWeatherMap.Model.QuickType;

namespace OpenWeatherMap
{
    public class OpenWeatherMap
    {
        private readonly string _apiKey;

        public OpenWeatherMap(string apiKey)
        {
            _apiKey = apiKey;
        }
        public async Task<Weather> GetCurrent(string cityName)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric";

            var weather = await _get<Weather>(url);

            return weather;
        }

        async Task<T> _get<T>(string url)
            where T : class
        {
            url += $"&appid={_apiKey}";

            var c = new HttpClient();
            var result = await c.GetAsync(new Uri(url));

            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var resultJson = await result.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<T>(resultJson);

            return obj;
        }
    } 
}
