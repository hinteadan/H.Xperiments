using H.Necessaire;

namespace H.MQ.FileSystem.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .Register<FileSystemHmqExternalEventListener>(() => new FileSystemHmqExternalEventListener())
                ;
        }
    }
}
