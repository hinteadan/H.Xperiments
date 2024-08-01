using H.MQ.Abstractions;
using H.Necessaire;

namespace H.MQ.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<Storage.DependencyGroup>(() => new Storage.DependencyGroup())

                .Register<ImAnHmqEventRegistry>(() => new HmqEventRegistry())
                .Register<ImAnHmqActor>(() => new HmqActor())

                ;
        }
    }
}
