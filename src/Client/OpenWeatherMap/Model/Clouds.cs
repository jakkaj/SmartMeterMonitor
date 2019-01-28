using Newtonsoft.Json;

namespace OpenWeatherMap.Model
{
    public partial class Clouds
    {
        [JsonProperty("all")]
        public long All { get; set; }
    }
}