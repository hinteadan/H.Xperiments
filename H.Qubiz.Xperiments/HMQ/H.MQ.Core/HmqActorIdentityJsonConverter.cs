using H.MQ.Abstractions;
using Newtonsoft.Json.Converters;
using System;

namespace H.MQ.Core
{
    internal class HmqActorIdentityJsonConverter : CustomCreationConverter<ImAnHmqActorIdentity>
    {
        public override ImAnHmqActorIdentity Create(Type objectType) => new HmqActorIdentity();
    }
}
