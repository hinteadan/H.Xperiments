using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.NamedPipe.Concrete
{
    [ID("NamedPipe")]
    [Alias("named-pipe", "np")]
    internal class NamedPipeHmqExternalEventListener : ImAnHmqExternalEventListener, ImADependency, IDisposable
    {
        const string defaultPipeName = "hmq";
        string pipeName = defaultPipeName;
        ImAnHmqEventRiser internalEventRiser;
        ImALogger logger;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            ConfigNode config
                = dependencyProvider
                .GetRuntimeConfig()
                ?.Get("HMQ")
                ?.Get("NamedPipe")
                ;
            string configPipeName = config?.Get("PipeName")?.ToString();
            if (!configPipeName.IsEmpty())
                pipeName = configPipeName;

            internalEventRiser = dependencyProvider.Build<ImAnHmqEventRiser>("internal");
            logger = dependencyProvider.GetLogger<NamedPipeHmqExternalEventListener>();
        }

        public Task<OperationResult> Start()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Stop()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            new Action(() =>
            {
                Stop().ConfigureAwait(false).GetAwaiter().GetResult();

            }).TryOrFailWithGrace();
        }
    }
}
