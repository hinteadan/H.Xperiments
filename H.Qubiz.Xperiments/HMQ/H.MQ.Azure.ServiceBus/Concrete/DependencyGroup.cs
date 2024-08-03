using H.MQ.Abstractions;
using H.Necessaire;

namespace H.MQ.Azure.ServiceBus.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<AzureServiceBusHmqEventRegistry>(() => new AzureServiceBusHmqEventRegistry())
                .Register<ImAnHmqEventRegistry>(() => dependencyRegistry.Get<AzureServiceBusHmqEventRegistry>())
                .Register<ImAnHmqEventReActionRegistry>(() => dependencyRegistry.Get<AzureServiceBusHmqEventRegistry>())

                .Register<AzureServiceBusHmqEventRiser>(() => new AzureServiceBusHmqEventRiser())

                .Register<AzureServiceBusHmqExternalEventListener>(() => new AzureServiceBusHmqExternalEventListener())

                ;
        }
    }
}
