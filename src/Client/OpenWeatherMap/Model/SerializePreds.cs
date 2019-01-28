using Newtonsoft.Json;

namespace OpenWeatherMap.Model
{
    public static class SerializePreds
    {
        public static string ToJson(this Prediction self) => JsonConvert.SerializeObject((object) self, (JsonSerializerSettings) ConverterPreds.Settings);
    }
}