using System;
using Newtonsoft.Json;

namespace Xamariners.Mobile.Core.Common.Json
{
    public class CharEnumConverter<TEnum> : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;

            if (string.IsNullOrWhiteSpace(enumString) || enumString.Length > 1)
                return null;
            
            return Enum.ToObject(typeof(TEnum), enumString[0]).ToString();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
