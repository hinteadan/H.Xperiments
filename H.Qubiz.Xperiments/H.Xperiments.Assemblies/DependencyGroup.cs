using H.Necessaire;

namespace H.Xperiments.Assemblies
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .RegisterAlwaysNew<AssemblyCommand>(() => new AssemblyCommand())
                .RegisterAlwaysNew<DebugSubCommand>(() => new DebugSubCommand())
                ;
        }
    }
}
