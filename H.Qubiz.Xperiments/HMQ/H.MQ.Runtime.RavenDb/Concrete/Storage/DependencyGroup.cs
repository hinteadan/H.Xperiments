using H.MQ.Abstractions;
using H.Necessaire;
using System;

namespace H.MQ.Runtime.RavenDb.Concrete.Storage
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<RavenDbHmqEventStorageService>(() => new RavenDbHmqEventStorageService())
                .Register<ImAStorageService<Guid, HmqEvent>>(() => dependencyRegistry.Get<RavenDbHmqEventStorageService>())
                .Register<ImAStorageBrowserService<HmqEvent, HmqEventFilter>>(() => dependencyRegistry.Get<RavenDbHmqEventStorageService>())

                .Register<RavenDbHmqEventReActionStorageService>(() => new RavenDbHmqEventReActionStorageService())
                .Register<ImAStorageService<Guid, HmqEventReactionLog>>(() => dependencyRegistry.Get<RavenDbHmqEventReActionStorageService>())
                .Register<ImAStorageBrowserService<HmqEventReactionLog, HmqEventReActionFilter>>(() => dependencyRegistry.Get<RavenDbHmqEventReActionStorageService>())

                ;
        }
    }
}
