using H.MQ.Abstractions;
using H.Necessaire.RavenDB;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using System;
using System.Linq;

namespace H.MQ.RavenDB.Concrete.Storage
{
    internal class HmqEventsRavenDbStorageService
        : RavenDbStorageResourceBase<Guid, HmqEvent, HmqEventFilter, HmqEventsFilterIndex>
    {
        protected override Guid GetIdFor(HmqEvent item) => item.ID;

        protected override IRavenQueryable<HmqEvent> ApplyFilter(IRavenQueryable<HmqEvent> query, HmqEventFilter filter)
        {
            if (filter?.IDs?.Any() == true)
                query = query.Where(x => x.ID.In(filter.IDs));

            if (filter?.From != null)
                query = query.Where(x => x.HappenedAt >= filter.From);

            if (filter?.To != null)
                query = query.Where(x => x.HappenedAt <= filter.To);

            if (filter?.Names?.Any() == true)
                query = query.Where(x => x.Name.In(filter.Names));

            if (filter?.Types?.Any() == true)
                query = query.Where(x => x.Type.In(filter.Types));

            if (filter?.Assemblies?.Any() == true)
                query = query.Where(x => x.Assembly.In(filter.Assemblies));

            return query;
        }
    }

    internal class HmqEventsFilterIndex : AbstractIndexCreationTask<HmqEvent>
    {
        public HmqEventsFilterIndex()
        {
            Map = docs => docs.Select(doc =>
                new
                {
                    doc.ID,
                    doc.HappenedAt,
                    doc.Name,
                    doc.Type,
                    doc.Assembly,
                }
            );
        }
    }
}
