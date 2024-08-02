using H.Necessaire;

namespace H.MQ.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<Storage.DependencyGroup>(() => new Storage.DependencyGroup())

                .Register<HmqEventRegistry>(() => new HmqEventRegistry())
                .Register<ImAnHmqEventRegistry>(() => dependencyRegistry.Get<HmqEventRegistry>())
                .Register<ImAnHmqEventReActionRegistry>(() => dependencyRegistry.Get<HmqEventRegistry>())

                .Register<ImAnHmqActorAndReActorBookkeeper>(() => new HmqActorAndReActorBookkeeper())

                .Register<ImAnHmqEventRiser>(() => new HmqEventRiser())

                .Register<PeriodicPollingHmqExternalEventReceiver>(() => new PeriodicPollingHmqExternalEventReceiver())

                .RegisterAlwaysNew<HmqActor>(() => new HmqActor())
                .RegisterAlwaysNew<HmqReActor>(() => new HmqReActor())

                ;
        }
    }
}
