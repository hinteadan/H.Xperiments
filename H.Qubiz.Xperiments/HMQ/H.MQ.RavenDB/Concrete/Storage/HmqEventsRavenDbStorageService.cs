using H.MQ.Abstractions;
using H.Necessaire.RavenDB;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using System.Linq;

namespace H.MQ.RavenDB.Concrete.Storage
{
    internal class HmqEventsRavenDbStorageService
        : RavenDbStorageResourceBase<string, ServiceBusMessage, HmqEventFilter, HmqEventsFilterIndex>
    {
        protected override string GetIdFor(ServiceBusMessage item) => item.ID;

        protected override IRavenQueryable<ServiceBusMessage> ApplyFilter(IRavenQueryable<ServiceBusMessage> query, HmqEventFilter filter)
        {
            if (filter?.IDs?.Any() == true)
                query = query.Where(x => x.Event.ID.In(filter.IDs));

            if (filter?.From != null)
                query = query.Where(x => x.Event.HappenedAt >= filter.From);

            if (filter?.To != null)
                query = query.Where(x => x.Event.HappenedAt <= filter.To);

            if (filter?.Names?.Any() == true)
                query = query.Where(x => x.Event.Name.In(filter.Names));

            if (filter?.Types?.Any() == true)
                query = query.Where(x => x.Event.Type.In(filter.Types));

            if (filter?.Assemblies?.Any() == true)
                query = query.Where(x => x.Event.Assembly.In(filter.Assemblies));

            return query;
        }
    }

    internal class HmqEventsFilterIndex : AbstractIndexCreationTask<ServiceBusMessage>
    {
        public HmqEventsFilterIndex()
        {
            Map = docs => docs.Select(doc =>
                new
                {
                    doc.Event.ID,
                    doc.Event.HappenedAt,
                    doc.Event.Name,
                    doc.Event.Type,
                    doc.Event.Assembly,
                }
            );
        }
    }
}
