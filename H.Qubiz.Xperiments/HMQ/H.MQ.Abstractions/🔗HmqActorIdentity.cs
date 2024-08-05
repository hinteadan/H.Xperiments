using H.Necessaire;
using System;

namespace H.MQ.Abstractions
{
    public class HmqActorIdentity : ImAnHmqActorIdentity
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public Note[] IdentityAttributes { get; set; }
    }
}
