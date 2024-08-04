using H.MQ.Abstractions;
using H.MQ.FileSystem.Concrete;
using H.Necessaire;

namespace H.MQ.RavenDB.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .Register<Storage.DependencyGroup>(() => new Storage.DependencyGroup())

                .Register<RavenDbHmqEventRiser>(() => new RavenDbHmqEventRiser())
                .Register<ImAnHmqEventRiser>(() => dependencyRegistry.Get<RavenDbHmqEventRiser>())

                .Register<RavenDbHmqExternalEventListener>(() => new RavenDbHmqExternalEventListener())

                ;
        }
    }
}
