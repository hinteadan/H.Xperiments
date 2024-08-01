using H.MQ.Abstractions;
using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqActor : ImAnHmqActor, ImADependency
    {
        ImAnHmqEventRegistry eventRegistry;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            eventRegistry = dependencyProvider.Get<ImAnHmqEventRegistry>();
        }

        public async Task<OperationResult> Raise(ImAnHmqEvent hmqEvent)
        {
            return await eventRegistry.Append(hmqEvent);
        }
    }
}
