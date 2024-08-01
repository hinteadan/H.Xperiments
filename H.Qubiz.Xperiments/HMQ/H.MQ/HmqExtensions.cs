using H.MQ.Abstractions;
using H.MQ.Concrete;
using H.Necessaire;
using System;

namespace H.MQ
{
    public static class HmqExtensions
    {
        public static ImAnHmqEvent ToHmqEvent<T>(this T data, params Note[] attributes)
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
    }
}
