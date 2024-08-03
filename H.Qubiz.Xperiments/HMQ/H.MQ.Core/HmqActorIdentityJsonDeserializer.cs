using H.MQ.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace H.MQ.Core
{
    internal class HmqActorIdentityJsonDeserializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException("HmqActorIdentityJsonConverter should only be used while deserializing.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            HmqActorIdentity value = serializer.Deserialize<HmqActorIdentity>(reader);

            return value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ImAnHmqActorIdentity);
        }

        public override bool CanWrite => false;
    }
}
