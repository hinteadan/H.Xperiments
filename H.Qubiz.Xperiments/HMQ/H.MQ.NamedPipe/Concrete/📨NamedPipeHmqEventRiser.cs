using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.NamedPipe.Concrete
{
    internal class NamedPipeHmqEventRiser : ImAnHmqEventRiser
    {
        public Task<OperationResult<ImAnHmqReActor>[]> Raise(HmqEvent hmqEvent)
        {
            throw new NotImplementedException();
        }
    }
}
