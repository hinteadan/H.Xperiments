using H.MQ.Abstractions;
using H.Necessaire;
using System;

namespace H.MQ
{
    public class HmqEventFilter : SortFilterBase, IPageFilter
    {
        static readonly string[] validSortNames = new string[] {
            nameof(HmqEvent.ID),
            nameof(HmqEvent.HappenedAt),
            nameof(HmqEvent.Name),
            nameof(HmqEvent.Assembly),
            nameof(HmqEvent.Type),
        };

        public Guid[] IDs { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public string[] Names { get; set; }
        public string[] Types { get; set; }
        public string[] Assemblies { get; set; }

        public PageFilter PageFilter { get; set; }

        protected override string[] ValidSortNames => validSortNames;
    }
}