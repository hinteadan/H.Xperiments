using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace H.Xperiments.Azf.Runtime.Debug
{
    public class Debug
    {
        static int numberOfTimesConstructorWasCalled = 0;
        static int numberOfTimesRunWasCalled = 0;

        private readonly ILogger<Debug> _logger;
        private readonly InstanceCountingService scopedInstanceCountingService;
        private readonly InstanceCountingService singletonInstanceCountingService;
        private readonly InstanceCountingService transientInstanceCountingService;

        public Debug(
            ILogger<Debug> logger,
            [FromKeyedServices("ScopedInstanceCountingService")] InstanceCountingService scopedInstanceCountingService,
            [FromKeyedServices("SingletonInstanceCountingService")] InstanceCountingService singletonInstanceCountingService,
            [FromKeyedServices("TransientInstanceCountingService")] InstanceCountingService transientInstanceCountingService
        )
        {
            _logger = logger;
            logger.LogInformation($"Debug Constructor called {Interlocked.Increment(ref numberOfTimesConstructorWasCalled)} time(s)");

            this.scopedInstanceCountingService = scopedInstanceCountingService.CountInjection();
            this.singletonInstanceCountingService = singletonInstanceCountingService.CountInjection();
            this.transientInstanceCountingService = transientInstanceCountingService.CountInjection();
        }

        [Function(nameof(Debug))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult(new DebugResponse
            {
                ConstructionCount = numberOfTimesConstructorWasCalled,
                RunCount = Interlocked.Increment(ref numberOfTimesRunWasCalled),
                ScopedService = new DebugResponse.ServiceInfo { 
                    InstanceID = scopedInstanceCountingService.GetInfo().InstanceID,
                    InjectionCount = scopedInstanceCountingService.GetInfo().InjectionCount,
                },
                SingletonService = new DebugResponse.ServiceInfo
                {
                    InstanceID = singletonInstanceCountingService.GetInfo().InstanceID,
                    InjectionCount = singletonInstanceCountingService.GetInfo().InjectionCount,
                },
                TransientService = new DebugResponse.ServiceInfo
                {
                    InstanceID = transientInstanceCountingService.GetInfo().InstanceID,
                    InjectionCount = transientInstanceCountingService.GetInfo().InjectionCount,
                },
            });
        }

        class DebugResponse
        {
            public int ConstructionCount { get; set; }
            public int RunCount { get; set; }
            public ServiceInfo SingletonService { get; set; }
            public ServiceInfo ScopedService { get; set; }
            public ServiceInfo TransientService { get; set; }

            public class ServiceInfo
            {
                public int InstanceID { get; set; }
                public int InjectionCount { get; set; }
            }
        }
    }
}
