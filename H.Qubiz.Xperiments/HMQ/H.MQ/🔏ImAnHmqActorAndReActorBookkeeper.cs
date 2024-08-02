using H.MQ.Abstractions;
using System;

namespace H.MQ
{
    internal interface ImAnHmqActorAndReActorBookkeeper
    {
        ImAnHmqActor GetOrAddActor(string id, Func<string, ImAnHmqActor> actorBuilder);
        ImAnHmqReActor GetOrAddReActor(string id, Func<string, ImAnHmqReActor> reActorBuilder);
        ImAnHmqReActor[] GetAllReActors();
    }
}
