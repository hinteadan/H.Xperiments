using H.Necessaire;

namespace H.Qubiz.Xperiments.CLI
{
    internal class CLIDependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .Register<RavenDb.DependencyGroup>(() => new RavenDb.DependencyGroup())
                .Register<SQLite.DependencyGroup>(() => new SQLite.DependencyGroup())
                .Register<H.Xperiments.Assemblies.DependencyGroup>(() => new H.Xperiments.Assemblies.DependencyGroup())
                .Register<H.Xperiments.DotNetStuff.DependencyGroup>(() => new H.Xperiments.DotNetStuff.DependencyGroup())
                ;
        }
    }
}
