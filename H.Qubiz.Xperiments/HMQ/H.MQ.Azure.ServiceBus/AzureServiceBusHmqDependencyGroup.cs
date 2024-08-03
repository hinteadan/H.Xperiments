using H.Necessaire;

namespace H.MQ.Azure.ServiceBus
{
    internal class AzureServiceBusHmqDependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<Concrete.DependencyGroup>(() => new Concrete.DependencyGroup())

                ;
        }
    }
}