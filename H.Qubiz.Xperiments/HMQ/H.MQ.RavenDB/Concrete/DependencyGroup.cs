using H.Necessaire;

namespace H.MQ.RavenDB.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .Register<Storage.DependencyGroup>(() => new Storage.DependencyGroup())

                .Register<RavenDbHmqExternalEventListener>(() => new RavenDbHmqExternalEventListener())

                ;
        }
    }
}
