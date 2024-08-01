using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.Abstractions
{
    public interface ImAnHmqReActor
    {
        Task<OperationResult> Handle(ImAnHmqEvent hmqEvent);
    }
}
