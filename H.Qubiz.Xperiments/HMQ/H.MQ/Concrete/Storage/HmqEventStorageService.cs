using H.Necessaire;
using System;
using System.Collections.Generic;
using System.Linq;

namespace H.MQ.Concrete.Storage
{
    internal class HmqEventStorageService : InMemoryStorageServiceBase<Guid, HmqEvent, HmqEventFilter>
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
                stream = stream.Where(x => x.Name.In(filter.Names, (item, key) =>  item.Is(key)));

            if (filter?.Types?.Any() == true)
                stream = stream.Where(x => x.Type.In(filter.Types, (item, key) => item.Is(key)));

            if (filter?.Assemblies?.Any() == true)
                stream = stream.Where(x => x.Assembly.In(filter.Assemblies, (item, key) => item.Is(key)));

            return stream;
        }
    }
}
