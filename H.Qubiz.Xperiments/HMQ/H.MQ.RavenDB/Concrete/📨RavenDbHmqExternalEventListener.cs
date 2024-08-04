using H.MQ.Abstractions;
using H.MQ.RavenDB.Concrete.Storage;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.RavenDB.Concrete
{
    [ID("RavenDB")]
    [Alias("raven")]
    internal class RavenDbHmqExternalEventListener : ImAnHmqExternalEventListener, ImADependency, IDisposable
    {
        RavenDbServiceBusStorageService ravenDbServiceBusStore;
        ImAnHmqEventRiser internalEventRiser;
        ImALogger logger;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            ravenDbServiceBusStore = dependencyProvider.Get<RavenDbServiceBusStorageService>();
            internalEventRiser = dependencyProvider.Build<ImAnHmqEventRiser>("internal");
            logger = dependencyProvider.GetLogger<RavenDbHmqExternalEventListener>();
        }

        public Task<OperationResult> Start()
        {
            ravenDbServiceBusStore.OnServiceBusMessage += RavenDbServiceBusStore_OnServiceBusMessage;
            return OperationResult.Win().AsTask();
        }

        public Task<OperationResult> Stop()
        {
            ravenDbServiceBusStore.OnServiceBusMessage -= RavenDbServiceBusStore_OnServiceBusMessage;
            ravenDbServiceBusStore.Dispose();
            return OperationResult.Win().AsTask();
        }

        public void Dispose()
        {
            new Action(() =>
            {
                Stop().ConfigureAwait(false).GetAwaiter().GetResult();

            }).TryOrFailWithGrace();
        }

        private async void RavenDbServiceBusStore_OnServiceBusMessage(object sender, RavenDbServiceBusMessageEventArgs e)
        {
            if (e.ServiceBusMessageID.IsEmpty())
                return;

            await
                new Func<Task>(async () =>
                {
                    ServiceBusMessage serviceBusMessage = await ravenDbServiceBusStore.Load(e.ServiceBusMessageID);

                    if (serviceBusMessage?.Event is null)
                    {
                        await logger.LogError($"RavenDB service bus event for {e.ServiceBusMessageID} is empty.");
                        return;
                    }

                    HmqEvent hmqEvent = serviceBusMessage.Event;

                    await internalEventRiser.Raise(hmqEvent);

                })
                .TryOrFailWithGrace(onFail: async ex =>
                {
                    await logger.LogError($"Error occured while handling RavenDB service bus event for {e.ServiceBusMessageID}. Message: {ex.Message}", ex);
                });
        }
    }
}
