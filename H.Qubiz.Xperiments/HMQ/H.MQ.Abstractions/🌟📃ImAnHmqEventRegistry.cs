using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.Abstractions
{
    public interface ImAnHmqEventRegistry
    {
        Task<OperationResult> Append(HmqEvent hmqEvent);

        Task<OperationResult<IDisposableEnumerable<HmqEvent>>> StreamAll();

        Task<OperationResult<IDisposableEnumerable<HmqEvent>>> Stream(HmqEventFilter filter);
    }
}
