using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    [Alias("np")]
    internal class NamedPipesCommand : CommandBase
    {
        static readonly string[] usageSyntax = [
            "np|NamedPipes serve|start",
            "np|NamedPipes stop|end",
            "np|NamedPipes send <message:string>",
        ];
        protected override string[] GetUsageSyntaxes() => usageSyntax;

        public override Task<OperationResult> Run() => RunSubCommand();

        [Alias("start")]
        class ServeSubCommand : SubCommandBase
        {
            public override void ReferDependencies(ImADependencyProvider dependencyProvider)
            {
                base.ReferDependencies(dependencyProvider);
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            }

            private async void CurrentDomain_ProcessExit(object sender, EventArgs e)
            {
                await State.Stop();
            }

            private async Task OnMessageReceived(string message)
            {
                await Task.CompletedTask;
                Log($"Named pipe message received: {message}");
            }

            public override async Task<OperationResult> Run(params Note[] args)
            {
                if (State.IsRunning)
                    return OperationResult.Win();

                State.IsRunning = true;

                await Task.CompletedTask;

                Log($"Starting to serve Named Pipes under {State.PipeName}...");
                using (new TimeMeasurement(x => Log($"Serving Named Pipes under {State.PipeName} in {x}")))
                {
                    StartNewServerAndWaitForClientConnection();
                }

                return OperationResult.Win();
            }

            void StartNewServerAndWaitForClientConnection()
            {
                Task.Run(async () =>
                {
                    using (NamedPipeServerStream namedPipeServerStream = new NamedPipeServerStream(State.PipeName, PipeDirection.InOut, -1))
                    {

                        await namedPipeServerStream.WaitForConnectionAsync(State.CancellationTokenSource.Token);

                        StartNewServerAndWaitForClientConnection();

                        Log($"Named Pipe client connected on {State.PipeName}");

                        StringBuilder messageBuilder = new StringBuilder();

                        byte[] readBuffer = new byte[State.BufferSize];
                        while (namedPipeServerStream.IsConnected && !State.CancellationTokenSource.IsCancellationRequested)
                        {
                            int readBytes = await namedPipeServerStream.ReadAsync(readBuffer, 0, readBuffer.Length, State.CancellationTokenSource.Token);
                            if (readBytes == 0)
                            {
                                await Task.Delay(State.PipeReadPause, State.CancellationTokenSource.Token);
                                if (State.CancellationTokenSource.IsCancellationRequested)
                                    break;
                                else
                                    continue;
                            }

                            string messageChunk = Encoding.UTF8.GetString(readBuffer, 0, readBytes);
                            messageBuilder.Append(messageChunk);
                        }

                        namedPipeServerStream.Close();

                        string message = messageBuilder.ToString();

                        await OnMessageReceived(message);
                    }
                },
                State.CancellationTokenSource.Token);
            }
        }

        [Alias("end")]
        class StopSubCommand : SubCommandBase
        {
            public override async Task<OperationResult> Run(params Note[] args)
            {
                await Task.CompletedTask;
                await State.Stop();
                return OperationResult.Win();
            }
        }

        class SendSubCommand : SubCommandBase
        {
            public override async Task<OperationResult> Run(params Note[] args)
            {
                string messageToSend = string.Join(" ", args?.Select(x => x.ID) ?? []);
                if (messageToSend.IsEmpty())
                    return OperationResult.Fail("No message specified");

                using NamedPipeClientStream namedPipeClientStream
                    = new NamedPipeClientStream(".", State.PipeName, PipeDirection.InOut, PipeOptions.None, System.Security.Principal.TokenImpersonationLevel.None);

                Log($"Connecting to named pipe {State.PipeName}");

                await namedPipeClientStream.ConnectAsync(State.CancellationTokenSource.Token);

                byte[] messageToSendAsBytes = Encoding.UTF8.GetBytes(messageToSend);

                await namedPipeClientStream.WriteAsync(messageToSendAsBytes, 0, messageToSendAsBytes.Length, State.CancellationTokenSource.Token);

                namedPipeClientStream.Close();

                return OperationResult.Win();
            }
        }

        static class State
        {
            public static bool IsRunning { get; set; } = false;

            public static readonly TimeSpan PipeReadPause = TimeSpan.FromSeconds(.15);
            public const uint BufferSize = 256;
            public const string PipeName = "H.Necessaire.IPC.Pipe";
            public static CancellationTokenSource CancellationTokenSource { get; private set; } = new CancellationTokenSource();

            public static async Task Stop()
            {
                await CancellationTokenSource.CancelAsync();
                CancellationTokenSource = new CancellationTokenSource();
                IsRunning = false;
            }
        }
    }
}
