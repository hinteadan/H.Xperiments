using H.MQ.Abstractions;
using H.Necessaire;

namespace H.MQ.Azure.ServiceBus
{
    public static class HmqIoCExtensions
    {
        public static T WithAzureServiceBusHmq<T>(this T dependencyRegistry) where T : ImADependencyRegistry
        {
            dependencyRegistry.Register<AzureServiceBusHmqDependencyGroup>(() => new AzureServiceBusHmqDependencyGroup());
            return dependencyRegistry;
        }

        public static T StartHmqAzureServiceBusExternalListener<T>(this T dependencyProvider) where T : ImADependencyProvider
            => dependencyProvider.StartHmqExternalListener("AzureServiceBus");

        private static T StartHmqExternalListener<T>(this T dependencyProvider, string buildTypeID) where T : ImADependencyProvider
        {
            ImAnHmqExternalEventListener periodicPollingExternalListener
                = dependencyProvider.Build<ImAnHmqExternalEventListener>(buildTypeID);
            periodicPollingExternalListener.Start();
            return dependencyProvider;
        }
    }
}
