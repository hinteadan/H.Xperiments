using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using H.Necessaire.WebSockets;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    [Alias("ws")]
    internal class WebSocketCommand : CommandBase
    {
        IWebSocketServerService webSocketServerService;
        public override void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            base.ReferDependencies(dependencyProvider);

            webSocketServerService = dependencyProvider.Get<IWebSocketServerService>();
        }

        public override async Task<OperationResult> Run()
        {
            Log("WebSocket-ing...");
            using (new TimeMeasurement(x => Log($"DONE WebSocket-ing in {x}")))
            {
                await Task.CompletedTask;

                await webSocketServerService.StartAsync(CancellationToken.None);

                Log("Running WSS Server on ws://localhost:11080");

                Console.ReadLine();

                await webSocketServerService.StopAsync(CancellationToken.None);

                Log("Stopped WSS Server");
            }

            return OperationResult.Win();
        }
    }
}
