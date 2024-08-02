using H.MQ.Abstractions;
using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ
{
    internal interface ImAnHmqEventReActingRegistry
    {
        Task<OperationResult> LogEventReAction(ImAnHmqEvent hmqEvent, OperationResult<ImAnHmqActorIdentity>[] hmqReActorResults); 
    }
}
