using H.MQ.Abstractions;
using H.Necessaire;
using System;

namespace H.MQ.Concrete.Storage.Model
{
    public class HmqEventReactionLog : IGuidIdentity
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public DateTime AsOf { get; set; } = DateTime.UtcNow;

        public HmqEvent Event { get; set; }
        public Guid EventID => Event?.ID ?? Guid.Empty;
        public DateTime EventHappenedAt => Event?.HappenedAt ?? AsOf;

        public ImAnHmqActorIdentity ActorIdentity { get; set; }
        public string ActorID => ActorIdentity?.ID;

        public OperationResult OperationResult { get; set; }
        public bool IsSuccessful => OperationResult?.IsSuccessful == true;
    }
}
