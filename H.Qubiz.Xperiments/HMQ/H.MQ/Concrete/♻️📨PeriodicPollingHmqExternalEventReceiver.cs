using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    [ID("PeriodicPolling")]
    [Alias("Periodic-Polling", "polling", "poll", "pull")]
    internal class PeriodicPollingHmqExternalEventReceiver : ImAnHmqExternalEventListener, ImADependency
    {
        ImAnHmqEventRegistry eventRegistry;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            eventRegistry = dependencyProvider.Get<ImAnHmqEventRegistry>();
        }

        public async Task<OperationResult> Start()
        {
            throw new System.NotImplementedException();
        }

        public async Task<OperationResult> Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}
