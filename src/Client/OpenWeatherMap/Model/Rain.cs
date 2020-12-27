using Newtonsoft.Json;

namespace OpenWeatherMap.Model
{
    public partial class Rain
    {
        [JsonProperty("3h", NullValueHandling = NullValueHandling.Ignore)]
        public double? The3H { get; set; }
    }
}