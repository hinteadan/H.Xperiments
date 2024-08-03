using H.MQ.Abstractions;
using H.Necessaire;
using System;

namespace H.MQ.Core
{
    internal class HmqActorIdentity : ImAnHmqActorIdentity
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public Note[] IdentityAttributes { get; set; }
    }
}
