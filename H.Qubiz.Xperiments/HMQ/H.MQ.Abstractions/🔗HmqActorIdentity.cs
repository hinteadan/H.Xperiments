using H.MQ.Abstractions;
using H.Necessaire;
using System;

namespace H.MQ.Core
{
    public class HmqActorIdentity : ImAnHmqActorIdentity
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public Note[] IdentityAttributes { get; set; }
    }
}
