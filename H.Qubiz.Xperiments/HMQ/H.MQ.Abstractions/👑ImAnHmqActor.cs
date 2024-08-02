using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.Abstractions
{
    public interface ImAnHmqActor : ImAnHmqActorIdentity
    {
        Task<OperationResult> Raise(HmqEvent hmqEvent);
    }
}
