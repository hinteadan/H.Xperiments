using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqReActor : HmqActorIdentity, ImAnHmqReActor, ImADependency
    {
        public Task<OperationResult> Handle(HmqEvent hmqEvent)
        {
            throw new NotImplementedException();
        }

        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            throw new NotImplementedException();
        }
    }
}
