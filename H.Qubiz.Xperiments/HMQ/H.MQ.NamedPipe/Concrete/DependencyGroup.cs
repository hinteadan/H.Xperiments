using H.MQ.Abstractions;
using H.Necessaire;

namespace H.MQ.NamedPipe.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<NamedPipeHmqEventRiser>(() => new NamedPipeHmqEventRiser())
                .Register<ImAnHmqEventRiser>(() => dependencyRegistry.Get<NamedPipeHmqEventRiser>())

                .Register<NamedPipeHmqExternalEventListener>(() => new NamedPipeHmqExternalEventListener())

                ;
        }
    }
}
