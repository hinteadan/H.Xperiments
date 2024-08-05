using H.MQ.Abstractions;
using H.Necessaire;

namespace H.MQ.RabbitMQ.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<RabbitMqHmqEventRiser>(() => new RabbitMqHmqEventRiser())
                .Register<ImAnHmqEventRiser>(() => dependencyRegistry.Get<RabbitMqHmqEventRiser>())

                .Register<RabbitMqHmqExternalEventListener>(() => new RabbitMqHmqExternalEventListener())

                ;
        }
    }
}
