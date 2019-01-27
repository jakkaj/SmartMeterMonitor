using Newtonsoft.Json;
using OpenWeatherMap.Model.QuickType;

namespace OpenWeatherMap.Model
{
    public static class Serialize
    {
        public static string ToJson(this Weather self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}