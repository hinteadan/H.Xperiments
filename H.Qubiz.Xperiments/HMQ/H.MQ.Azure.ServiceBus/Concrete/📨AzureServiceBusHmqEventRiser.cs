using H.MQ.Abstractions;
using H.MQ.Core;
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

        public async Task<OperationResult<ImAnHmqReActor>[]> Raise(HmqEvent hmqEvent)
        {
            if (hmqEvent is null)
                return Array.Empty<OperationResult<ImAnHmqReActor>>();

            HmqEvent eventToSave = hmqEvent.Clone().MarkAsPersisted();
            throw new NotImplementedException();
        }
    }
}
