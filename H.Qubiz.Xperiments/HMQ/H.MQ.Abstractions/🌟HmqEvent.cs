using H.Necessaire;
using System;

namespace H.MQ.Abstractions
{
    public sealed class HmqEvent : IGuidIdentity
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        public DateTime HappenedAt { get; set; } = DateTime.UtcNow;


        public string Name { get; set; }

        public string Type { get; set; }

        public string Assembly { get; set; }


        public Note[] Attributes { get; set; }


        public object Data { get; set; }
    }
}
