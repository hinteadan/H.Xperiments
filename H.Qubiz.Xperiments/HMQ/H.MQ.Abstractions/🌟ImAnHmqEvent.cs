using H.Necessaire;
using System;

namespace H.MQ.Abstractions
{
    public interface ImAnHmqEvent : IGuidIdentity
    {
        DateTime HappenedAt { get; }

        string Name { get; }
        string Type { get; }
        string Assembly { get; }

        Note[] Attributes { get; }

        object Data { get; }
    }
}