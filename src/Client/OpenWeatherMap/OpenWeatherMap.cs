using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenWeatherMap.Model;


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

            var weatherJson = await _getString(url);

            var weather = Weather.FromJson(weatherJson);

            return weather;
        }

        public async Task<Prediction> GetForecast(string cityName)
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={cityName}&units=metric";

            var predJson = await _getString(url);

            var prediction = Prediction.FromJson(predJson);

            return prediction;
        }

        public async Task<(double today, double tomorrow)> GetMinimums(string cityName)
        {
            var city = await GetForecast(cityName);

            var dtoToday = new DateTimeOffset(DateTime.Today.ToUniversalTime());
            var dtoTomorrow = new DateTimeOffset(DateTime.Today.AddDays(1).ToUniversalTime());
            var dtoDayAfterTomorrow = new DateTimeOffset(DateTime.Today.AddDays(2).ToUniversalTime());

            var todayMax = city.List.Where
                (_ => _.Dt >= dtoToday.ToUnixTimeSeconds() &&
                      _.Dt <= dtoTomorrow.ToUnixTimeSeconds())
                .Min(_ => _.Main.Temp);

            var tomorrowMax = city.List.Where
                (_ => _.Dt >= dtoTomorrow.ToUnixTimeSeconds() &&
                      _.Dt <= dtoDayAfterTomorrow.ToUnixTimeSeconds())
                .Min(_ => _.Main.Temp);

            return (todayMax, tomorrowMax);
        }

        public async Task<(double today, double tomorrow)> GetMaximums(string cityName)
        {
            var city = await GetForecast(cityName);

            var dtoToday = new DateTimeOffset(DateTime.Today.ToUniversalTime());
            var dtoTomorrow = new DateTimeOffset(DateTime.Today.AddDays(1).ToUniversalTime());
            var dtoDayAfterTomorrow = new DateTimeOffset(DateTime.Today.AddDays(2).ToUniversalTime());

            var todayMax = city.List.Where
                (_ => _.Dt >= dtoToday.ToUnixTimeSeconds() &&
                      _.Dt <= dtoTomorrow.ToUnixTimeSeconds())
                .Max(_ => _.Main.Temp);

            var tomorrowMax = city.List.Where
                (_ => _.Dt >= dtoTomorrow.ToUnixTimeSeconds() &&
                      _.Dt <= dtoDayAfterTomorrow.ToUnixTimeSeconds())
                .Max(_ => _.Main.Temp);

            return (todayMax, tomorrowMax);
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

        async Task<string> _getString(string url)
        {
            url += $"&appid={_apiKey}";

            var c = new HttpClient();
            var result = await c.GetAsync(new Uri(url));

            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var resultJson = await result.Content.ReadAsStringAsync();
            return resultJson;
        }
    } 
}
