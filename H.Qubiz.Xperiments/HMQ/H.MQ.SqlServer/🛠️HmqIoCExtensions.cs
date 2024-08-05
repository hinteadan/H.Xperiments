using H.MQ.Core;
using H.Necessaire;

namespace H.MQ.SqlServer
{
    public static class HmqIoCExtensions
    {
        public static T WithHmqRavenDbMessageBus<T>(this T dependencyRegistry) where T : ImADependencyRegistry
        {
            dependencyRegistry.Register<DependencyGroup>(() => new DependencyGroup());
            return dependencyRegistry;
        }

        public static T StartHmqSqlServerExternalListener<T>(this T dependencyProvider) where T : ImADependencyProvider
            => dependencyProvider.StartHmqExternalListener("SqlServer");
    }
}
