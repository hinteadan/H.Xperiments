using H.MQ.Abstractions;
using H.MQ.Core;
using H.MQ.FileSystem.Concrete.Storage;
using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.FileSystem.Concrete
{
    internal class FileSystemHmqEventRiser : ImAnHmqEventRiser, ImADependency
    {
        HmqEventsJsonCachedFileSystemStorageService hmqEventsFileSystem;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            hmqEventsFileSystem = dependencyProvider.Get<HmqEventsJsonCachedFileSystemStorageService>();
        }

        public async Task<OperationResult<ImAnHmqReActor>[]> Raise(HmqEvent hmqEvent)
        {
            HmqEvent eventToSend = hmqEvent.Clone().MarkAsPersisted();

            OperationResult result = await hmqEventsFileSystem.Save(eventToSend);

            return result.WithPayload(FileSystemReActor.Instance).AsArray();
        }
    }
}
