using H.MQ.Abstractions;
using H.Necessaire;

namespace H.MQ.Azure.ServiceBus.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                //.Register<HmqEventRegistry>(() => new HmqEventRegistry())
                //.Register<ImAnHmqEventRegistry>(() => dependencyRegistry.Get<HmqEventRegistry>())
                //.Register<ImAnHmqEventReActionRegistry>(() => dependencyRegistry.Get<HmqEventRegistry>())

                .Register<ImAnHmqEventRiser>(() => new AzureServiceBusHmqEventRiser())

                .Register<AzureServiceBusHmqExternalEventListener>(() => new AzureServiceBusHmqExternalEventListener())

                ;
        }
    }
}
