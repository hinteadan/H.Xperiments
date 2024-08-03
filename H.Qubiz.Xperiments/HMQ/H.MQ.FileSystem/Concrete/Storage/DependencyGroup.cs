using H.Necessaire;

namespace H.MQ.FileSystem.Concrete.Storage
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<HmqEventsJsonCachedFileSystemStorageService>(() => new HmqEventsJsonCachedFileSystemStorageService())

                ;
        }
    }
}
