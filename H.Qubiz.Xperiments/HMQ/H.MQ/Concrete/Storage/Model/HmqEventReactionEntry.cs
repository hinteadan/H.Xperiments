using H.Necessaire;
using System;

namespace H.MQ.Concrete.Storage.Model
{
    internal class HmqEventReactionEntry : IGuidIdentity
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public DateTime AsOf { get; set; } = DateTime.UtcNow;


    }
}
