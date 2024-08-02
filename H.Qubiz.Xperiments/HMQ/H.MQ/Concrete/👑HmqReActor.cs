using H.MQ.Abstractions;
using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqReActor : HmqActorIdentity, ImAnHmqReActor, ImADependency
    {
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            
        }

        public async Task<OperationResult> Handle(HmqEvent hmqEvent)
        {
            
        }
    }
}
