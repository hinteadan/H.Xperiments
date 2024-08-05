using H.MQ.Core;
using H.Necessaire;

namespace H.MQ.RavenDB
{
    public static class HmqIoCExtensions
    {
        public static T WithHmqRavenDbMessageBus<T>(this T dependencyRegistry) where T : ImADependencyRegistry
        {
            dependencyRegistry.Register<RavenDbHmqDependencyGroup>(() => new RavenDbHmqDependencyGroup());
            return dependencyRegistry;
        }

        public static T StartHmqRavenDbExternalListener<T>(this T dependencyProvider) where T : ImADependencyProvider
            => dependencyProvider.StartHmqExternalListener("RavenDB");
    }
}
