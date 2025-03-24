using H.Necessaire;

namespace H.Xperiments.Couchbase
{
    class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .RegisterAlwaysNew<CouchbaseCommand>(() => new CouchbaseCommand())
                ;
        }
    }
}
