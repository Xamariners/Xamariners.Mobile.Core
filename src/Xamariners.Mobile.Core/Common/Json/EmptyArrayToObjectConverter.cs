using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Xamariners.Mobile.Core.Common.Json
{
    /// <summary>
    /// Handles the case where the api returns an object when there is an item e.g.
    /// "cart": { //object }
    /// And an empty array when there are no items e.g.
    /// "cart": []
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Newtonsoft.Json.Converters.CustomCreationConverter{T}" />
    public class EmptyArrayToObjectConverter<T> : CustomCreationConverter<T> where T : class, new()
    {
        public override T Create(Type objectType)
        {
            return new T();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read();

                if (reader.TokenType == JsonToken.EndArray)
                    return new T();

                throw new JsonSerializationException($"Cannot convert non-empty JSON Array to an object type {typeof(T)}");
            }

            return serializer.Deserialize(reader, objectType);
        }
    }
}
