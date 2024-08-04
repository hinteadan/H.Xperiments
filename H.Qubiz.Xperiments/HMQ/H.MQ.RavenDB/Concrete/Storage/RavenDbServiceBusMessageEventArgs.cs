using System;

namespace H.MQ.RavenDB.Concrete.Storage
{
    internal class RavenDbServiceBusMessageEventArgs : EventArgs
    {
        public RavenDbServiceBusMessageEventArgs(Guid eventID)
        {
            EventID = eventID;
        }

        public Guid EventID { get; }
    }
}
