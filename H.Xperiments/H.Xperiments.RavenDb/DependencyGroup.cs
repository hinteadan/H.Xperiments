using H.Necessaire;

namespace H.Xperiments.RavenDb
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .RegisterAlwaysNew<RavenDbCommand>(() => new RavenDbCommand())
                .RegisterAlwaysNew<ServeEmbeddedSubCommand>(() => new ServeEmbeddedSubCommand())
                ;
        }
    }
}
