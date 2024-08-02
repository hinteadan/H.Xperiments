using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.Abstractions
{
    public interface ImAnHmqReActor : ImAnHmqActorIdentity
    {
        Task<OperationResult> Handle(HmqEvent hmqEvent);
    }
}
