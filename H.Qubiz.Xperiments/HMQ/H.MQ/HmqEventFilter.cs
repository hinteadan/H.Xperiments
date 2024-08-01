using System;

namespace H.MQ
{
    public class HmqEventFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public string[] Names { get; set; }
        public string[] Types { get; set; }
        public string[] Assemblies { get; set; }
    }
}