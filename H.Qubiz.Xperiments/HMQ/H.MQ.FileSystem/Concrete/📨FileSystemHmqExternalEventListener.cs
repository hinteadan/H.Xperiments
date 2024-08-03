using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.FileSystem.Concrete
{
    [ID("FileSystem")]
    [Alias("fs")]
    internal class FileSystemHmqExternalEventListener : ImAnHmqExternalEventListener, ImADependency, IDisposable
    {
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Start()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Stop()
        {
            throw new NotImplementedException();
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
