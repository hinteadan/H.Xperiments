using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ
{
    internal interface ImAnHmqExternalEventListener
    {
        Task<OperationResult> Start();

        Task<OperationResult> Stop();
    }
}
