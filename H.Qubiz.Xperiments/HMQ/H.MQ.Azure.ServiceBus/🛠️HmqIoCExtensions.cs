using H.MQ.Core;
using H.Necessaire;

namespace H.MQ.Azure.ServiceBus
{
    public static class HmqIoCExtensions
    {
        public static T WithHmqAzureServiceBusMessageBus<T>(this T dependencyRegistry) where T : ImADependencyRegistry
        {
            dependencyRegistry.Register<AzureServiceBusHmqDependencyGroup>(() => new AzureServiceBusHmqDependencyGroup());
            return dependencyRegistry;
        }

        public static T StartHmqAzureServiceBusExternalListener<T>(this T dependencyProvider) where T : ImADependencyProvider
            => dependencyProvider.StartHmqExternalListener("AzureServiceBus");
    }
}
