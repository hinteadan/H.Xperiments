using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.Abstractions
{
    public interface ImAnHmqEventReActionRegistry
    {
        Task<OperationResult> LogEventReAction(HmqEvent hmqEvent, params OperationResult<ImAnHmqActorIdentity>[] hmqReActorResults);
        Task<OperationResult<IDisposableEnumerable<HmqEventReactionLog>>> StreamAll();
        Task<OperationResult<IDisposableEnumerable<HmqEventReactionLog>>> Stream(HmqEventReActionFilter filter);
    }
}
