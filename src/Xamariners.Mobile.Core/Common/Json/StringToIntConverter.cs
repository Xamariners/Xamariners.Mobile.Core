using Newtonsoft.Json;
using System;
using Xamariners.Mobile.Core.Extensions;

namespace Xamariners.Mobile.Core.Common.Json
{
    public class StringToIntConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType.IsIntegerType();
        public override bool CanWrite => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    return Convert.ToInt32(reader.Value);
                case JsonToken.String:
                    string value = reader.Value as string;
                    int.TryParse(value, out var result);
                    return result;
                case JsonToken.Null:
                    return 0;
                default:
                    throw new JsonSerializationException($"Cannot convert \"{reader.Value}\" of type {reader.TokenType} to integer.");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is int)
            {
                writer.WriteValue(Convert.ToString(value));
            }
            else
            {
                throw new JsonSerializationException($"The property type for value \"{value}\" is not integer.");
            }
        }
    }

}
