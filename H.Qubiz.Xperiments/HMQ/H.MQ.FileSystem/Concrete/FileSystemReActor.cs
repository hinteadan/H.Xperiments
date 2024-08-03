using H.MQ.Abstractions;
using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.FileSystem.Concrete
{
    internal class FileSystemReActor : ImAnHmqReActor
    {
        public static readonly ImAnHmqReActor Instance = new FileSystemReActor();

        const string id = "FileSystemReActor-{05CCB6A1-275B-40A5-B7C2-0B97A344E457}";

        public Note[] IdentityAttributes { get; set; }

        public string ID => id;

        public Task<OperationResult> Handle(HmqEvent hmqEvent)
        {
            return OperationResult.Win().AsTask();
        }
    }
}
