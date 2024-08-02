using H.MQ.Abstractions;
using H.Necessaire;

namespace H.MQ.Concrete
{
    internal class HmqActorIdentity : ImAnHmqActorIdentity
    {
        public string ID { get; set; }
        public Note[] IdentityAttributes { get; set; }
    }
}
