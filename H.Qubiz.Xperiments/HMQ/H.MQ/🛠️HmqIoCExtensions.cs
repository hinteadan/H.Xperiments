using H.Necessaire;

namespace H.MQ
{
    public static class HmqIoCExtensions
    {
        public static T WithHmq<T>(this T dependencyRegistry) where T : ImADependencyRegistry
        {
            dependencyRegistry.Register<HmqDependencyGroup>(() => new HmqDependencyGroup());
            return dependencyRegistry;
        }

        public static T StartHmqPeriodicPollingExternalListener<T>(this T dependencyProvider) where T : ImADependencyProvider
        {
            ImAnHmqExternalEventListener periodicPollingExternalListener
                = dependencyProvider.Build<ImAnHmqExternalEventListener>("PeriodicPolling");
            periodicPollingExternalListener.Start();
            return dependencyProvider;
        }
    }
}
