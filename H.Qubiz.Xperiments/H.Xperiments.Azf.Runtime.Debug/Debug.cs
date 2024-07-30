using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace H.Xperiments.Azf.Runtime.Debug
{
    public class Debug
    {
        private readonly ILogger<Debug> _logger;

        public Debug(ILogger<Debug> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Debug))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
