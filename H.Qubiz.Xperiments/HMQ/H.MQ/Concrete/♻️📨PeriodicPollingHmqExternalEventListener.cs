using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    [ID("PeriodicPolling")]
    [Alias("Periodic-Polling", "polling", "poll", "pull")]
    internal class PeriodicPollingHmqExternalEventListener : ImAnHmqExternalEventListener, ImADependency
    {
        static readonly TimeSpan pollingInterval = TimeSpan.FromSeconds(5);
        static readonly TimeSpan intervalToLookBackInto = TimeSpan.FromDays(21);
        ImAnHmqEventRegistry eventRegistry;
        ImAnHmqEventReActionRegistry eventReActionRegistry;
        ImAnHmqEventRiser eventRiser;
        ImAPeriodicAction poller;
        ImALogger logger;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            eventRegistry = dependencyProvider.Get<ImAnHmqEventRegistry>();
            eventReActionRegistry = dependencyProvider.Get<ImAnHmqEventReActionRegistry>();
            eventRiser = dependencyProvider.Get<ImAnHmqEventRiser>();
            poller = poller ?? dependencyProvider.Get<ImAPeriodicAction>();
            logger = dependencyProvider.GetLogger<PeriodicPollingHmqExternalEventListener>();
        }

        public Task<OperationResult> Start()
        {
            poller.Start(pollingInterval, RunPollingSession);
            return OperationResult.Win().AsTask();
        }

        public Task<OperationResult> Stop()
        {
            poller.Stop();
            return OperationResult.Win().AsTask();
        }

        async Task RunPollingSession()
        {
            OperationResult<IDisposableEnumerable<HmqEvent>> eventsStreamResult
                = await eventRegistry.Stream(new HmqEventFilter
                { 
                    From = DateTime.UtcNow - intervalToLookBackInto,
                });

            if (!eventsStreamResult.IsSuccessful)
            {
                await logger.LogError($"Error ocurred while trying to stream events from the past {intervalToLookBackInto.TotalDays} days", new OperationResultException(eventsStreamResult), payload: null, eventsStreamResult.FlattenReasons().ToNotes().NullIfEmpty());
                return;
            }

            using (IDisposableEnumerable<HmqEvent> stream = eventsStreamResult.Payload)
            {
                foreach (HmqEvent hmqEvent in stream)
                {
                    await eventRiser.Raise(hmqEvent);
                }
            }
        }
    }
}
