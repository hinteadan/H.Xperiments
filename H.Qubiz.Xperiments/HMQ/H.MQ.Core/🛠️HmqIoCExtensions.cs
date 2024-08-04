using H.MQ.Abstractions;
using H.Necessaire;

namespace H.MQ.Core
{
    public static class HmqIoCExtensions
    {
        public static T StartHmqExternalListener<T>(this T dependencyProvider, string buildTypeID) where T : ImADependencyProvider
        {
            ImAnHmqExternalEventListener periodicPollingExternalListener
                = dependencyProvider.Build<ImAnHmqExternalEventListener>(buildTypeID);
            periodicPollingExternalListener.Start().ConfigureAwait(false).GetAwaiter().GetResult();
            return dependencyProvider;
        }
    }
}
