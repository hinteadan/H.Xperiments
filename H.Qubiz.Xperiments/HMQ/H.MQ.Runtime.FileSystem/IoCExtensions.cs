using H.Necessaire;

namespace H.MQ.Runtime.FileSystem
{
    public static class IoCExtensions
    {
        public static T WithHmqFileSystemRuntime<T>(this T dependencyRegistry) where T : ImADependencyRegistry
        {
            dependencyRegistry.Register<DependencyGroup>(() => new DependencyGroup());
            return dependencyRegistry;
        }
    }
}
