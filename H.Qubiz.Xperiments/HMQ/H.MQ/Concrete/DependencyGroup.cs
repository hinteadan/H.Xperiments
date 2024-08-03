using H.MQ.Abstractions;
using H.Necessaire;

namespace H.MQ.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<ImAnHmqActorAndReActorBookkeeper>(() => new HmqActorAndReActorBookkeeper())
                .RegisterAlwaysNew<HmqActor>(() => new HmqActor())
                .RegisterAlwaysNew<HmqReActor>(() => new HmqReActor())

                .Register<Storage.DependencyGroup>(() => new Storage.DependencyGroup())

                .Register<HmqEventRegistry>(() => new HmqEventRegistry())
                .Register<ImAnHmqEventRegistry>(() => dependencyRegistry.Get<HmqEventRegistry>())
                .Register<ImAnHmqEventReActionRegistry>(() => dependencyRegistry.Get<HmqEventRegistry>())

                .Register<ImAnHmqEventRiser>(() => new HmqEventRiser())

                .Register<PeriodicPollingHmqExternalEventListener>(() => new PeriodicPollingHmqExternalEventListener())

                ;
        }
    }
}
