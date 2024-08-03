using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.Abstractions
{
    public interface ImAnHmqEventRiser
    {
        Task<OperationResult<ImAnHmqReActor>[]> Raise(HmqEvent hmqEvent);
    }
}
