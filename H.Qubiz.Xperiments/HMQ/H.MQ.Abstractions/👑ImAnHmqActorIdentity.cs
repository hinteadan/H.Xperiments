using H.Necessaire;

namespace H.MQ.Abstractions
{
    public interface ImAnHmqActorIdentity : IStringIdentity
    {
        Note[] IdentityAttributes { get; }
    }
}
