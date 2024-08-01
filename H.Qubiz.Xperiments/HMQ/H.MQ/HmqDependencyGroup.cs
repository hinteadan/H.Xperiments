using H.MQ.Concrete.Storage;
using H.Necessaire;
using System;

namespace H.MQ
{
    internal class HmqDependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<HmqEventStorageService>(() => new HmqEventStorageService())
                .Register<ImAStorageService<Guid, HmqEvent>>(() => dependencyRegistry.Get<HmqEventStorageService>())
                .Register<ImAStorageBrowserService<HmqEvent, HmqEventFilter>>(() => dependencyRegistry.Get<HmqEventStorageService>())

                .Register<ImAnHmqEventRegistry>(() => new Concrete.HmqEventRegistry())
                ;
        }
    }
}
