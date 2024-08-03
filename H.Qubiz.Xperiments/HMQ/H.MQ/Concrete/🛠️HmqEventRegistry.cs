using H.MQ.Core;
using H.Necessaire;

namespace H.MQ.Concrete
{
    [ID("InternalEventRegistry")]
    [Alias("internal", "internal-event-registry")]
    internal class HmqEventRegistry : HmqEventRegistryBackedByStorageServicesBase
    {

    }
}
