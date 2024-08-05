using H.Necessaire;

namespace H.MQ.Runtime.RavenDb.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .Register<Storage.DependencyGroup>(() => new Storage.DependencyGroup())
                ;
        }
    }
}
