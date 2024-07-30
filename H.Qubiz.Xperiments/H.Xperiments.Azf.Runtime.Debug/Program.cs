using H.Xperiments.Azf.Runtime.Debug;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddKeyedScoped<InstanceCountingService>("ScopedInstanceCountingService");
        services.AddKeyedSingleton<InstanceCountingService>("SingletonInstanceCountingService");
        services.AddKeyedTransient<InstanceCountingService>("TransientInstanceCountingService");
    })
    .Build();

host.Run();
