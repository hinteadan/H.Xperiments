using H.Necessaire;
using H.Necessaire.Runtime.SqlServer;

namespace H.MQ.Runtime.SqlServer
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .Register<SqlServerRuntimeDependencyGroup>(() => new SqlServerRuntimeDependencyGroup())
                .Register<Concrete.DependencyGroup>(() => new Concrete.DependencyGroup())
                ;
        }
    }
}
