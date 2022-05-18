using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xamariners.Mobile.Core.Common.Json
{
    /// <summary>
    /// Handles a specific case where the api returns a dictionary when there are items e.g.
    /// "image_pairs": {
    ///     "90991": { //object }
    /// }
    /// And an empty array when there are no items e.g.
    /// "image_pairs: []
    /// </summary>
    public class DictionaryConverter<TKey, TValue> : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read();

                if (reader.TokenType == JsonToken.EndArray)
                    return new Dictionary<TKey, TValue>();
                else
                {
                    // TODO: This(else) can be deleted once client resove the issue with products in order GetOrders for 
                    // order id : 1800760, 1800458, 1800298 orders with mismatched type(array instead of dictionary)
                    // findings : the item id of the products in these 3 orders is 0

                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        var initialDepth = reader.Depth;
                        while (reader.Read() && reader.Depth > initialDepth)
                        {
                            //skip the object in the array
                        }
                        
                        //skip the end array character
                        reader.Read();
                        return new Dictionary<TKey, TValue>();
                    }
                }
                throw new JsonSerializationException("Cannot convert non-empty JSON Array to a Dictionary");
            }

            if (reader.TokenType == JsonToken.StartObject)
            {
                var textWriter = new System.IO.StringWriter();
                var jsonWriter = new JsonTextWriter(textWriter);

                jsonWriter.WriteStartObject();

                var initialDepth = reader.Depth;
                while (reader.Read() && reader.Depth > initialDepth)
                {
                    jsonWriter.WriteToken(reader);
                }

                jsonWriter.WriteEndObject();
                jsonWriter.Flush();

                return JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(textWriter.ToString());
            }

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<TKey, TValue>);
        }
    }
}
