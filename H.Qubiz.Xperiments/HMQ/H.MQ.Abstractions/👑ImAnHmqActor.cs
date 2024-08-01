using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.Abstractions
{
    public interface ImAnHmqActor
    {
        Task<OperationResult> Raise(ImAnHmqEvent hmqEvent);
    }
}
