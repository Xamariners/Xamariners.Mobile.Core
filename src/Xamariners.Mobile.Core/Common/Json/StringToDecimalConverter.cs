using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xamariners.Mobile.Core.Extensions;

namespace Xamariners.Mobile.Core.Common.Json
{
    public class StringToDecimalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal);
        }
        public override bool CanWrite => true;
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                    string value = reader.Value as string;
                    decimal.TryParse(value, out var result);
                    return result;
                case JsonToken.Null:
                    return 0;
                default:
                    throw new JsonSerializationException($"Cannot convert \"{reader.Value}\" of type {reader.TokenType} to decimal.");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is decimal)
            {
                writer.WriteValue(Convert.ToString(value));
            }
            else
            {
                throw new JsonSerializationException($"The property type for value \"{value}\" is not decimal.");
            }
        }
    }
}
