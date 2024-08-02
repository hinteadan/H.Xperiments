using H.MQ.Abstractions;
using H.MQ.Concrete;
using H.Necessaire;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace H.MQ
{
    public static class HmqExtensions
    {
        static readonly string[] externalSourceNames = new string[] { "PersistentStore" };

        public static HmqEvent ToHmqEvent<T>(this T data, params Note[] attributes)
        {
            Type dataType = data?.GetType() ?? typeof(T);

            return
                new HmqEvent
                {
                    ID = Guid.NewGuid(),

                    HappenedAt = DateTime.UtcNow,

                    Assembly = dataType.Assembly.FullName,
                    Type = dataType.FullName,
                    Name = dataType.Name,

                    Attributes = attributes.NullIfEmpty(),

                    Data = data,
                };
        }

        public static HmqEvent Clone(this HmqEvent hmqEvent)
        {
            if (hmqEvent is null)
                return null;

            return
                new HmqEvent
                {
                    ID = hmqEvent.ID,
                    Assembly = hmqEvent.Assembly,
                    Type = hmqEvent.Type,
                    Attributes = hmqEvent.Attributes.ToArrayNullIfEmpty(),
                    Data = hmqEvent.Data,
                    HappenedAt = hmqEvent.HappenedAt,
                    Name = hmqEvent.Name,
                };
        }

        public static bool IsExternal(this HmqEvent hmqEvent)
        {
            return hmqEvent?.Attributes?.Get("Source", ignoreCase: true)?.In(externalSourceNames, (val, key) => val.Is(key)) == true;
        }

        public static bool IsInternal(this HmqEvent hmqEvent)
        {
            return (hmqEvent?.Attributes?.Get("Source")).IsEmpty() || !hmqEvent.IsExternal();
        }



        public static ImAnHmqActorIdentity ToIdentityOnly<T>(this T identityHolder) where T : class, ImAnHmqActorIdentity
        {
            if (identityHolder is null)
                return null;

            return
                new HmqActorIdentity
                {
                    ID = identityHolder.ID,
                    IdentityAttributes = identityHolder.IdentityAttributes,
                };
        }



        public static T WithHmq<T>(this T dependencyRegistry) where T : ImADependencyRegistry
        {
            dependencyRegistry.Register<HmqDependencyGroup>(() => new HmqDependencyGroup());
            return dependencyRegistry;
        }

        public static T StartHmqPeriodicPollingExternalListener<T>(this T dependencyProvider) where T : ImADependencyProvider
        {
            ImAnHmqExternalEventListener periodicPollingExternalListener
                = dependencyProvider.Build<ImAnHmqExternalEventListener>("PeriodicPolling");
            periodicPollingExternalListener.Start();
            return dependencyProvider;
        }

        public static ImAnHmqActor GetHmqActor(this ImADependencyProvider dependencyProvider, string id, params Note[] idAttrs)
        {
            if (id.IsEmpty())
                throw new ArgumentException($"HMQ Actor ID must be specified", nameof(id));

            ImAnHmqActorAndReActorBookkeeper actorAndReActorBookkeeper = dependencyProvider.Get<ImAnHmqActorAndReActorBookkeeper>();

            ImAnHmqActor actor = actorAndReActorBookkeeper.GetOrAddActor(id, i => dependencyProvider.Get<HmqActor>().And(a => {
                a.ID = i;
                a.IdentityAttributes = idAttrs?.Where(x => !x.IsEmpty())?.ToArrayNullIfEmpty();
            }));

            return actor;
        }

        public static ImAnHmqReActor GetHmqReActor(this ImADependencyProvider dependencyProvider, Func<HmqEvent, Task<OperationResult>> handler, string id, params Note[] idAttrs)
        {
            if (id.IsEmpty())
                throw new ArgumentException($"HMQ ReActor ID must be specified", nameof(id));

            ImAnHmqActorAndReActorBookkeeper actorAndReActorBookkeeper = dependencyProvider.Get<ImAnHmqActorAndReActorBookkeeper>();

            ImAnHmqReActor reActor = actorAndReActorBookkeeper.GetOrAddReActor(id, i => dependencyProvider.Get<HmqReActor>().And(r => {
                r.ID = i;
                r.IdentityAttributes = idAttrs?.Where(x => !x.IsEmpty())?.ToArrayNullIfEmpty();
                r.Handler = handler;
            }));
            
            return reActor;
        }
    }
}
