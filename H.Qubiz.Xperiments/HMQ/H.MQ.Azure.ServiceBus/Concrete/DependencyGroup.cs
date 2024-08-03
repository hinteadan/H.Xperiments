using H.Necessaire;

namespace H.MQ.Azure.ServiceBus.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<AzureServiceBusHmqExternalEventListener>(() => new AzureServiceBusHmqExternalEventListener())

                ;
        }
    }
}
