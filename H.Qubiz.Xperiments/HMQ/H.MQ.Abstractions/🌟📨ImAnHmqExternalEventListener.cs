using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.Abstractions
{
    public interface ImAnHmqExternalEventListener
    {
        Task<OperationResult> Start();

        Task<OperationResult> Stop();
    }
}
