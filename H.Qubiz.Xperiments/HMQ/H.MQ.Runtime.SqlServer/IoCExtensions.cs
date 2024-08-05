using H.Necessaire;

namespace H.MQ.Runtime.SqlServer
{
    public static class IoCExtensions
    {
        public static T WithHmqSqlServerRuntime<T>(this T dependencyRegistry) where T : ImADependencyRegistry
        {
            dependencyRegistry.Register<DependencyGroup>(() => new DependencyGroup());
            return dependencyRegistry;
        }
    }
}
