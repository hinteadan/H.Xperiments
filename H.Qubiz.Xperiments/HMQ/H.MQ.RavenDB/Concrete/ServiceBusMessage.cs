using H.MQ.Abstractions;
using H.Necessaire;
using System;

namespace H.MQ.RavenDB.Concrete
{
    internal class ServiceBusMessage : IStringIdentity
    {
        public string ID => $"ServiceBusMessage/{(Event?.ID) ?? Guid.NewGuid()}";

        public HmqEvent Event { get; set; }

        public static implicit operator ServiceBusMessage(HmqEvent @event) => new ServiceBusMessage { Event = @event };
        public static implicit operator HmqEvent(ServiceBusMessage serviceBusMessage) => serviceBusMessage?.Event;
    }
}
