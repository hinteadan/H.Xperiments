using H.MQ.Abstractions;
using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ
{
    internal interface ImAnHmqEventRiser
    {
        Task<OperationResult> Raise(ImAnHmqEvent hmqEvent);
    }
}
