using H.Necessaire;

namespace H.Qubiz.Xperiments.SQLite
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            //dependencyRegistry
            //    .Register<BLL.ArgCommands.DependencyGroup>(() => new BLL.DependencyGroup())
            //    .RegisterAlwaysNew<RavenDbCommand>(() => new RavenDbCommand())
            //    ;
        }
    }
}
