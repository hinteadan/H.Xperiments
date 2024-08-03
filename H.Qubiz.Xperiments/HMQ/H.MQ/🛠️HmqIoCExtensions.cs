using H.MQ.Core;
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
            => dependencyProvider.StartHmqExternalListener("PeriodicPolling");
    }
}
