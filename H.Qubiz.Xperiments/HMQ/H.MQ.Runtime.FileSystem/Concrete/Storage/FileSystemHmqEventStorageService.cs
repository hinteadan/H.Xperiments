using H.MQ.Abstractions;
using H.MQ.Core;
using H.Necessaire;
using H.Necessaire.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace H.MQ.Runtime.FileSystem.Concrete.Storage
{
    internal class FileSystemHmqEventStorageService : JsonCachedFileSystemStorageServiceBase<Guid, HmqEvent, HmqEventFilter>
    {
        protected override async Task<HmqEvent> ReadAndParseEntityFromStream(Stream serializedEntityStream)
        {
            HmqEvent hmqEvent = await base.ReadAndParseEntityFromStream(serializedEntityStream);
            hmqEvent = hmqEvent.ToWellTypedEventDataFromJson();
            return hmqEvent;
        }

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
    }
}
