using H.MQ.Core;
using H.Necessaire;

namespace H.MQ.FileSystem
{
    public static class HmqIoCExtensions
    {
        public static T WithHmqFileSystemMessageBus<T>(this T dependencyRegistry) where T : ImADependencyRegistry
        {
            dependencyRegistry.Register<DependencyGroup>(() => new DependencyGroup());
            return dependencyRegistry;
        }

        public static T StartHmqFileSystemExternalListener<T>(this T dependencyProvider) where T : ImADependencyProvider
            => dependencyProvider.StartHmqExternalListener("FileSystem");
    }
}
