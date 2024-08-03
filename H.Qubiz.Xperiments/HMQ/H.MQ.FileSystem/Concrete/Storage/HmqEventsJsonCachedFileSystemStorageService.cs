using H.MQ.Abstractions;
using H.Necessaire;
using H.Necessaire.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace H.MQ.FileSystem.Concrete.Storage
{
    internal class HmqEventsJsonCachedFileSystemStorageService : CachedFileSystemStorageServiceBase<Guid, HmqEvent, HmqEventFilter>
    {
        protected override IEnumerable<HmqEvent> ApplyFilter(IEnumerable<HmqEvent> stream, HmqEventFilter filter)
        {
            if (filter?.IDs?.Any() == true)
                stream = stream.Where(x => x.ID.In(filter.IDs));

            if (filter?.From != null)
                stream = stream.Where(x => x.HappenedAt >= filter.From);

            if (filter?.To != null)
                stream = stream.Where(x => x.HappenedAt <= filter.To);

            if (filter?.Names?.Any() == true)
                stream = stream.Where(x => x.Name.In(filter.Names, (item, key) => item.Is(key)));

            if (filter?.Types?.Any() == true)
                stream = stream.Where(x => x.Type.In(filter.Types, (item, key) => item.Is(key)));

            if (filter?.Assemblies?.Any() == true)
                stream = stream.Where(x => x.Assembly.In(filter.Assemblies, (item, key) => item.Is(key)));

            return stream;
        }

        protected override async Task<HmqEvent> ReadAndParseEntityFromStream(Stream serializedEntityStream)
        {
            return
                (await serializedEntityStream.ReadAsStringAsync())
                .TryJsonToObject<HmqEvent>()
                .ThrowOnFailOrReturn()
                ;
        }

        protected override async Task SerializeEntityToStream(HmqEvent entityToSerialize, Stream entitySerializationStream)
        {
            await
                entityToSerialize
                .ToJsonObject(isPrettyPrinted: true)
                .WriteToStreamAsync(entitySerializationStream)
                ;
        }
    }
}
