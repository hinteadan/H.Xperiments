using H.MQ.FileSystem.Concrete.Storage;
using H.Necessaire;

namespace H.MQ.FileSystem
{
    internal class FileSystemHmqDependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<DependencyGroup>(() => new Concrete.DependencyGroup())

                ;
        }
    }
}