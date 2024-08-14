using H.MQ.Core;
using H.Necessaire;

namespace H.MQ.NamedPipe
{
    public static class HmqIoCExtensions
    {
        public static T WithHmqNamedPipeMessageBus<T>(this T dependencyRegistry) where T : ImADependencyRegistry
        {
            dependencyRegistry.Register<DependencyGroup>(() => new DependencyGroup());
            return dependencyRegistry;
        }

        public static T StartHmqFileSystemExternalListener<T>(this T dependencyProvider) where T : ImADependencyProvider
            => dependencyProvider.StartHmqExternalListener("NamedPipe");
    }
}
