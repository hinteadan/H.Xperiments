using H.MQ.Abstractions;
using H.Necessaire;
using System;

namespace H.MQ
{
    public class HmqEvent : ImAnHmqEvent
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
