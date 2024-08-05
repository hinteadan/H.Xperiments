using H.Necessaire;
using H.Necessaire.RavenDB;

namespace H.MQ.RavenDB
{
    internal class DependencyGroup : RavenDbDependencyGroup
    {
        public override void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            base.RegisterDependencies(dependencyRegistry);

            dependencyRegistry

                .Register<Concrete.DependencyGroup>(() => new Concrete.DependencyGroup())

                ;
        }
    }
}