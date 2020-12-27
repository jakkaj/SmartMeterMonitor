using System;
using Newtonsoft.Json;

namespace OpenWeatherMap.Model
{
    internal class MainEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(MainEnum) || t == typeof(MainEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Clear":
                    return MainEnum.Clear;
                case "Clouds":
                    return MainEnum.Clouds;
                case "Rain":
                    return MainEnum.Rain;
            }
            throw new Exception("Cannot unmarshal type MainEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (MainEnum)untypedValue;
            switch (value)
            {
                case MainEnum.Clear:
                    serializer.Serialize(writer, "Clear");
                    return;
                case MainEnum.Clouds:
                    serializer.Serialize(writer, "Clouds");
                    return;
                case MainEnum.Rain:
                    serializer.Serialize(writer, "Rain");
                    return;
            }
            throw new Exception("Cannot marshal type MainEnum");
        }

        public static readonly MainEnumConverter Singleton = new MainEnumConverter();
    }
}