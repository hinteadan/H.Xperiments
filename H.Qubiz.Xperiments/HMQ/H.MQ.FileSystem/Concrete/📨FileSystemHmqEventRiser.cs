using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.FileSystem.Concrete
{
    internal class FileSystemHmqEventRiser : ImAnHmqEventRiser, ImADependency
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
