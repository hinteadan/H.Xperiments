using H.MQ.Abstractions;
using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ
{
    internal interface ImAnHmqEventRegistry
    {
        Task<OperationResult> Append(ImAnHmqEvent hmqEvent);

        Task<OperationResult<IDisposableEnumerable<ImAnHmqEvent>>> StreamAll();

        Task<OperationResult<IDisposableEnumerable<ImAnHmqEvent>>> Stream(HmqEventFilter filter);
    }
}
