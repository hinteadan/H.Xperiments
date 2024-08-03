using H.MQ.Abstractions;
using H.Necessaire;
using System;

namespace H.MQ.FileSystem.Concrete.Storage
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<HmqEventsJsonCachedFileSystemStorageService>(() => new HmqEventsJsonCachedFileSystemStorageService())
                .Register<ImAStorageService<Guid, HmqEvent>>(() => dependencyRegistry.Get<HmqEventsJsonCachedFileSystemStorageService>())
                .Register<ImAStorageBrowserService<HmqEvent, HmqEventFilter>>(() => dependencyRegistry.Get<HmqEventsJsonCachedFileSystemStorageService>())

                ;
        }
    }
}
