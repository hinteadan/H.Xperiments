using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace H.MQ.RabbitMQ.Concrete
{
    internal class RabbitMqHmqEventRiser : ImAnHmqEventRiser, ImADependency
    {
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<ImAnHmqReActor>[]> Raise(HmqEvent hmqEvent)
        {
            throw new NotImplementedException();
        }
    }
}
