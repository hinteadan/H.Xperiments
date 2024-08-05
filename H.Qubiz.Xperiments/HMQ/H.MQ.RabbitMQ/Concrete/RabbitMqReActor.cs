using H.MQ.Abstractions;
using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.RabbitMQ.Concrete
{
    internal class RabbitMqReActor : ImAnHmqReActor
    {
        public static readonly ImAnHmqReActor Instance = new RabbitMqReActor();

        const string id = "RabbitMqReActor-{AF0B1B76-269D-4527-8526-22F1F061441A}";

        public Note[] IdentityAttributes { get; set; }

        public string ID => id;

        public Task<OperationResult> Handle(HmqEvent hmqEvent)
        {
            return OperationResult.Win().AsTask();
        }
    }
}
