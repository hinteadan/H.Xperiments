using H.Necessaire;

namespace H.MQ.RavenDB.Concrete.Storage
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<HmqEventsRavenDbStorageService>(() => new HmqEventsRavenDbStorageService())

                ;
        }
    }
}
