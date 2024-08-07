using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using H.Necessaire.WebSockets;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    [Alias("ws")]
    internal class WebSocketCommand : CommandBase
    {
        public override void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            base.ReferDependencies(dependencyProvider);

            State.WebSocketServerService = dependencyProvider.Get<IWebSocketServerService>();
        }

        public override Task<OperationResult> Run() => RunSubCommand();

        [ID("send")]
        class SendSubCommand : SubCommandBase
        {
            IWebSocketServerToClientOperations clientNotifier;
            public override void ReferDependencies(ImADependencyProvider dependencyProvider)
            {
                base.ReferDependencies(dependencyProvider);
                clientNotifier = dependencyProvider.Get<IWebSocketServerToClientOperations>();
            }

            public override async Task<OperationResult> Run(params Note[] args)
            {
                Log("Sending WS Message...");
                using (new TimeMeasurement(x => Log($"DONE Sending WS Message in {x}")))
                {
                    if (!State.IsRunning)
                        return OperationResult.Win();

                    await clientNotifier.Broadcast(
                        new NotificationMessage { 
                            Content = args?.FirstOrDefault().ID,
                            Encoding = Encoding.UTF8,
                            ContentType = NotificationMessageContentType.Plain,
                            Subject = null,
                        },
                        new NotificationAddress { 
                            Address = "ws://localhost:11080",
                            Name = "WsDevTesting",
                        }
                    );

                    Log("Running WSS Server on ws://localhost:11080");
                }

                return OperationResult.Win();
            }
        }

        [ID("start")]
        class StartSubCommand : SubCommandBase
        {
            public override async Task<OperationResult> Run(params Note[] args)
            {
                Log("Starting WS Server...");
                using (new TimeMeasurement(x => Log($"DONE Starting WS Server in {x}")))
                {
                    await State.WebSocketServerService.StartAsync(CancellationToken.None);

                    State.IsRunning = true;

                    Log("Running WSS Server on ws://localhost:11080");
                }

                return OperationResult.Win();
            }
        }

        [ID("stop")]
        class StopSubCommand : SubCommandBase
        {
            public override async Task<OperationResult> Run(params Note[] args)
            {
                Log("Stopping WS Server...");
                using (new TimeMeasurement(x => Log($"DONE Stopping WS Server in {x}")))
                {
                    await State.WebSocketServerService.StopAsync(CancellationToken.None);

                    State.Clear();

                    Log("Running WSS Server on ws://localhost:11080");
                }

                return OperationResult.Win();
            }
        }


        static class State
        {
            public static IWebSocketServerService WebSocketServerService = null;

            public static bool IsRunning { get; set; } = false;

            public static void Clear()
            {
                IsRunning = false;
            }
        }
    }
}
