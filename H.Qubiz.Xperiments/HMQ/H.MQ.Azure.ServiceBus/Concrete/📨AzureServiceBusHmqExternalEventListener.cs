using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.Azure.ServiceBus.Concrete
{
    [ID("AzureServiceBus")]
    [Alias("azsb", "az-sb", "azure-sb", "azure-servicebus", "azure-service-bus")]
    internal class AzureServiceBusHmqExternalEventListener : ImAnHmqExternalEventListener, ImADependency
    {
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Start()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Stop()
        {
            throw new NotImplementedException();
        }
    }
}
