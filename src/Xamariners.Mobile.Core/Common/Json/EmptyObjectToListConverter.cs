using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xamariners.Mobile.Core.Common.Json
{
    /// <summary>
    /// Handles the case where the api returns an empty object when there are no items e.g.
    /// "orders": {}
    /// And an array of objects when there are items e.g.
    /// "orders": [{}]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EmptyObjectToListConverter<T> : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();

                return new List<T>();
            }

            return serializer.Deserialize(reader, objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}
