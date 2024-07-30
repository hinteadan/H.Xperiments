using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace H.Xperiments.Azf.Runtime.Debug
{
    public class Debug
    {
        static int numberOfTimesConstructorWasCalled = 0;
        static int numberOfTimesRunWasCalled = 0;

        private readonly ILogger<Debug> _logger;

        public Debug(ILogger<Debug> logger)
        {
            _logger = logger;
            logger.LogInformation($"Debug Constructor called {Interlocked.Increment(ref numberOfTimesConstructorWasCalled)} time(s)");
        }

        [Function(nameof(Debug))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult(new { ConstructionCount = numberOfTimesConstructorWasCalled, RunCount = Interlocked.Increment(ref numberOfTimesRunWasCalled) });
        }
    }
}
