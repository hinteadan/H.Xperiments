using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.RabbitMQ.Concrete
{
    [ID("RabbitMQ")]
    [Alias("rabbit-mq", "rabbit")]
    internal class RabbitMqHmqExternalEventListener : ImAnHmqExternalEventListener, ImADependency, IDisposable
    {
        ImAnHmqEventRiser internalEventRiser;
        ImALogger logger;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {

            internalEventRiser = dependencyProvider.Build<ImAnHmqEventRiser>("internal");
            logger = dependencyProvider.GetLogger<RabbitMqHmqExternalEventListener>();
        }

        public async Task<OperationResult> Start()
        {
            return OperationResult.Win();
        }

        public Task<OperationResult> Stop()
        {
            return OperationResult.Win().AsTask();
        }

        public void Dispose()
        {
            new Action(() =>
            {
                Stop().ConfigureAwait(false).GetAwaiter().GetResult();

            }).TryOrFailWithGrace();
        }
    }
}
