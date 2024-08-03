using H.MQ.Abstractions;
using H.Necessaire;

namespace H.MQ.Azure.ServiceBus.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<AzureServiceBusHmqEventRiser>(() => new AzureServiceBusHmqEventRiser())
                .Register<ImAnHmqEventRiser>(() => dependencyRegistry.Get<AzureServiceBusHmqEventRiser>())

                .Register<AzureServiceBusHmqExternalEventListener>(() => new AzureServiceBusHmqExternalEventListener())

                ;
        }
    }
}
