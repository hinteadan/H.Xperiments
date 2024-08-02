using H.MQ.Abstractions;
using H.MQ.Concrete;
using H.Necessaire;
using System;

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
    }
}
