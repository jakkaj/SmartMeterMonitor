using netatmocore.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NetatmoCore.Helpers;
using System.Globalization;

namespace NetatmoCore
{
    public class NetAtmoClient
    {
        /// <summary>
        /// Retrieved with NetatmoAuth class
        /// </summary>
        public string AccessToken { get; set; }

        private HttpClient httpClient;

        private const string BASE_URI = "https://api.netatmo.com/api/";

        public NetAtmoClient(string access_token)
        {
            AccessToken = access_token;
            Initialize();
        }

        protected void Initialize()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BASE_URI);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
        }

        /// <summary>
        /// Returns data from a user Weather Stations (measures and device specific data).
        /// <see cref="https://dev.netatmo.com/resources/technical/reference/weatherstation/getstationsdata"/>
        /// </summary>
        /// <returns></returns>
        public async Task<NAStationDataResponse> Getthermostatsdata(string deviceId)
        {
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("device_id", deviceId);
            var thermostatDataJson = await httpClient.PostAsync(UriHelper.ResourceUriBuilder(httpClient, "getstationsdata", AccessToken), null);

            if (!thermostatDataJson.IsSuccessStatusCode)
                throw new Exception("Server returned " + thermostatDataJson.StatusCode.ToString() + " - Check access_token and device_id parameter.");

            var thermostatData = JsonConvert.DeserializeObject<NAStationDataResponse>(await thermostatDataJson.Content.ReadAsStringAsync());

            if (thermostatData == null || thermostatData.Body == null)
                throw new Exception("Unable to deserialize reply into NAStationDataResponse. Possible version mismatch.");

            return thermostatData;
        }

        /// <summary>
        /// Returns public data from matching stations in given area.
        /// <see cref="https://dev.netatmo.com/en-US/resources/technical/reference/weatherapi/getpublicdata"/>
        /// </summary>
        /// <param name="latitude_ne"></param>
        /// <param name="longitude_ne"></param>
        /// <param name="latitude_sw"></param>
        /// <param name="longitude_sw"></param>
        /// <param name="required_data"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<NAPublicDataResponse> Getpublicdata(int latitude_ne, int longitude_ne, int latitude_sw, int longitude_sw, string[] required_data, bool filter)
        {
            var uri = UriHelper.ResourceUriBuilder(httpClient, "getpublicdata", AccessToken);
            uri.AddQuery("lat_ne", latitude_ne.ToString(CultureInfo.InvariantCulture))
                .AddQuery("lon_ne", longitude_ne.ToString(CultureInfo.InvariantCulture))
                .AddQuery("lat_sw", latitude_sw.ToString(CultureInfo.InvariantCulture))
                .AddQuery("lon_dw", longitude_sw.ToString(CultureInfo.InvariantCulture))
                .AddQuery("required_data", string.Join(",",required_data))
                .AddQuery("filter", filter.ToString(CultureInfo.InvariantCulture));

            var publicDataJson = await httpClient.GetAsync(uri);

            if (!publicDataJson.IsSuccessStatusCode)
                throw new Exception("Server returned " + publicDataJson.StatusCode.ToString() + " - Check access_token and device_id parameter.");

            var publicData = JsonConvert.DeserializeObject<NAPublicDataResponse>(await publicDataJson.Content.ReadAsStringAsync());

            if (publicData == null || publicData.Body == null)
                throw new Exception("Unable to deserialize reply into NAStationDataResponse. Possible version mismatch.");

            return publicData;
        }

    }
}
