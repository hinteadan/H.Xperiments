using H.MQ.Abstractions;
using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ
{
    internal interface ImAnHmqEventReActionRegistry
    {
        Task<OperationResult> LogEventReAction(HmqEvent hmqEvent, params OperationResult<ImAnHmqActorIdentity>[] hmqReActorResults); 
    }
}
