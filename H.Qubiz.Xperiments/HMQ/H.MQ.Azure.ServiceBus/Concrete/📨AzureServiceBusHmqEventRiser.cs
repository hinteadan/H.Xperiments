using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.Azure.ServiceBus.Concrete
{
    internal class AzureServiceBusHmqEventRiser : ImAnHmqEventRiser, ImADependency
    {
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<ImAnHmqReActor>[]> Raise(HmqEvent hmqEvent)
        {
            throw new NotImplementedException();
        }
    }
}
