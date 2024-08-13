using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System;
using System.Collections.Concurrent;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    [Alias("np")]
    internal class NamedPipesCommand : CommandBase
    {
        static readonly string[] usageSyntax = [
            "np|NamedPipes serve",
        ];

        public override Task<OperationResult> Run() => RunSubCommand();

        class ServeSubCommand : SubCommandBase
        {
            public override void ReferDependencies(ImADependencyProvider dependencyProvider)
            {
                base.ReferDependencies(dependencyProvider);
                State.OnMessageReceived += State_OnMessageReceived;
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            }

            private void CurrentDomain_ProcessExit(object sender, EventArgs e)
            {
                State.OnMessageReceived -= State_OnMessageReceived;
                State.Clear();
            }

            private void State_OnMessageReceived(object sender, PipeMessageReceivedEventArgs e)
            {
                Log($"Named pipe message received: {e.Message}");
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
                    State.Servers.Add(StartNewServerAndWaitForClientConnection());
                }

                return OperationResult.Win();
            }

            Task StartNewServerAndWaitForClientConnection()
            {
                return
                    Task.Run(async () =>
                    {
                        NamedPipeServerStream namedPipeServerStream = new NamedPipeServerStream(State.PipeName, PipeDirection.InOut);

                        await namedPipeServerStream.WaitForConnectionAsync(State.CancellationTokenSource.Token);

                        Log($"New Named Pipe client connected. {State.Servers.Count} in total.");

                        StringBuilder messageBuilder = new StringBuilder();

                        do
                        {
                            byte[] readBuffer = new byte[State.BufferSize];
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

                            int messageStartIndex = 0;
                            int messageEndIndex = messageChunk?.IndexOf(State.MessageEndMarker) ?? -1;

                            while(messageEndIndex >= 0)
                            {
                                messageBuilder.Append(messageChunk.Substring(messageStartIndex, messageEndIndex));
                                State.RaiseOnMessageReceived(messageBuilder.ToString());
                                messageBuilder = new StringBuilder();

                                messageStartIndex = messageEndIndex + 1;
                                if (messageStartIndex > messageChunk.Length - 1)
                                    break;

                                messageEndIndex = messageStartIndex > messageChunk.Length - 1 ? -1 : messageChunk.IndexOf(State.MessageEndMarker, messageStartIndex);
                                if (messageEndIndex < 0)
                                {
                                    messageBuilder.Append(messageChunk, messageStartIndex, messageChunk.Length - messageStartIndex);
                                }
                            }

                            if (messageStartIndex == 0)
                                messageBuilder.Append(messageChunk);

                        } while (!State.CancellationTokenSource.IsCancellationRequested);

                    },
                    State.CancellationTokenSource.Token);
            }
        }

        static class State
        {
            public static bool IsRunning { get; set; } = false;
            public static event EventHandler<PipeMessageReceivedEventArgs> OnMessageReceived;

            public static readonly TimeSpan PipeReadPause = TimeSpan.FromSeconds(.25);
            public const uint BufferSize = 1024;
            public const char MessageEndMarker = '\0';
            public const string PipeName = "H.Necessaire.IPC.Pipe";
            public static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
            public static readonly ConcurrentBag<Task> Servers = new ConcurrentBag<Task>();

            public static void RaiseOnMessageReceived(string message)
            {
                if (OnMessageReceived == null)
                    return;

                OnMessageReceived(null, new PipeMessageReceivedEventArgs(message));
            }

            public static void Clear()
            {
                new Action(() => {

                    foreach (var server in Servers)
                    {
                        server.Dispose();
                    }

                    Servers.Clear();

                }).TryOrFailWithGrace();

                IsRunning = false;
            }
        }

        class PipeMessageReceivedEventArgs : EventArgs
        {
            public PipeMessageReceivedEventArgs(string message)
            {
                Message = message;
            }

            public string Message { get; }
        }
    }
}
