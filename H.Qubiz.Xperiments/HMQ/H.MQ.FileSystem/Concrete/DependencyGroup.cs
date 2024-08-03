using H.MQ.Abstractions;
using H.Necessaire;

namespace H.MQ.FileSystem.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<Storage.DependencyGroup>(() => new Storage.DependencyGroup())

                .Register<FileSystemHmqEventRiser>(() => new FileSystemHmqEventRiser())
                .Register<ImAnHmqEventRiser>(() => dependencyRegistry.Get<FileSystemHmqEventRiser>())

                .Register<FileSystemHmqExternalEventListener>(() => new FileSystemHmqExternalEventListener())

                ;
        }
    }
}
