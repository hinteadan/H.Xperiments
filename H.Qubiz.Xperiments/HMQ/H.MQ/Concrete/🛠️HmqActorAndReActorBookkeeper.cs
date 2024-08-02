using H.MQ.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace H.MQ.Concrete
{
    internal class HmqActorAndReActorBookkeeper : ImAnHmqActorAndReActorBookkeeper
    {
        private readonly ConcurrentDictionary<string, ImAnHmqActor> actorsDictionary = new ConcurrentDictionary<string, ImAnHmqActor>();
        private readonly ConcurrentDictionary<string, ImAnHmqReActor> reActorsDictionary = new ConcurrentDictionary<string, ImAnHmqReActor>();

        public ImAnHmqReActor[] GetAllReActors() => reActorsDictionary.Values.ToArray();

        public ImAnHmqActor GetOrAddActor(string id, Func<string, ImAnHmqActor> actorBuilder)
            => actorsDictionary.GetOrAdd(id, actorBuilder);

        public ImAnHmqReActor GetOrAddReActor(string id, Func<string, ImAnHmqReActor> reActorBuilder)
            => reActorsDictionary.GetOrAdd(id, reActorBuilder);
    }
}
