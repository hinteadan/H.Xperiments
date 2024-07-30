using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using H.Necessaire.Serialization;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    [Alias("azf")]
    internal class AzureFunctionsCommand : CommandBase
    {
        static readonly string[] usageSyntax = [
            "azf|azurefunctions start|run|run-host|host|runhost [port=8898] [verbose] [no-new-window]",
            "azf|azurefunctions stop|end|kill|stop-host|stophost",
            "azf|azurefunctions call debug",
        ];
        protected override string[] GetUsageSyntaxes() => usageSyntax;

        public override Task<OperationResult> Run() => RunSubCommand();

        static class State
        {
            public static Process AzureFunctionsHostProcess = null;
            public static string AzureFunctionsBaseApiUrl = null;

            public static bool IsRunning => AzureFunctionsHostProcess is not null;

            public static void Clear()
            {
                AzureFunctionsHostProcess = null;
                AzureFunctionsBaseApiUrl = null;
            }
        }

        [ID("call")]
        class CallSubCommand : SubCommandBase
        {
            public override Task<OperationResult> Run(params Note[] args) => RunSubCommand(["".NoteAs("call"), ..args]);

            [ID("debug")]
            class DebugSubCommand : SubCommandBase
            {
                const int numberOfRequests = 10;
                static readonly HttpClient httpClient = new HttpClient();
                public override async Task<OperationResult> Run(params Note[] args)
                {
                    if (!State.IsRunning)
                        return OperationResult.Fail("AZF not running");

                    DebugResponse[] debugCallResults = await Task.WhenAll(
                        Enumerable.Range(0, numberOfRequests)
                        .Select(CallDebug)
                    ).ConfigureAwait(false);

                    var maxRunCount = debugCallResults.MaxBy(x => x.RunCount);
                    var maxConstructCount = debugCallResults.MaxBy(x => x.ConstructionCount);

                    return OperationResult.Win();
                }

                async Task<DebugResponse> CallDebug(int index)
                {
                    await Task.Delay(Random.Shared.Next(500, 2000));

                    string url = $"{State.AzureFunctionsBaseApiUrl}/debug";

                    using HttpResponseMessage response = (await httpClient.GetAsync(url).ConfigureAwait(false)).EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    DebugResponse debugResponse = responseContent.JsonToObject<DebugResponse>().And(x => x.Index = index);

                    return debugResponse;
                }

                class DebugResponse
                {
                    public int Index { get; set; } = -1;
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


        [Alias("stop", "end", "kill", "stop-host")]
        class StopHostSubCommand : SubCommandBase
        {
            public override Task<OperationResult> Run(params Note[] args)
            {
                if (!State.IsRunning)
                    return OperationResult.Win("AZF not running").AsTask();

                new Action(() => { 
                    State.AzureFunctionsHostProcess?.Kill(entireProcessTree: true);
                    State.Clear();
                    Log("AZF Host stopped");
                }).TryOrFailWithGrace(onFail: ex => Log($"Error occurred while trying to stop AZF Host. Message: {ex.Message}"));

                return OperationResult.Win().AsTask();
            }
        }

        [Alias("start", "run", "run-host", "host")]
        class RunHostSubCommand : SubCommandBase
        {
            private static readonly string srcFolderRelativePath = $"{Path.DirectorySeparatorChar}H.Qubiz.Xperiments{Path.DirectorySeparatorChar}";
            public override Task<OperationResult> Run(params Note[] args)
            {
                if (State.IsRunning)
                    return OperationResult.Fail("Already running AZF").AsTask();

                string port = args?.Get("port", ignoreCase: true);
                port ??= "7277";

                bool isVerbose = args?.Any(a => a.ID.Is("verbose")) == true;

                bool isSameWindow = args?.Any(a => a.ID.Is("no-new-window")) == true;

                string projectDirPath = Path.Combine(GetCodebaseFolderPath(), "H.Xperiments.Azf.Runtime.Debug");

                OperationResult result = OperationResult.Win();
                new Action(() =>
                {
                    State.AzureFunctionsHostProcess = Process.Start(new ProcessStartInfo
                    {
                        Arguments = $"start --port {port}{(isVerbose ? " --verbose" : "")}",
                        FileName = $"func",
                        WorkingDirectory = projectDirPath,
                        RedirectStandardOutput = isSameWindow,
                        RedirectStandardError = isSameWindow,
                        UseShellExecute = !isSameWindow,
                        CreateNoWindow = isSameWindow,
                        WindowStyle = isSameWindow ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
                    }).And(process => {
                        if (!isSameWindow)
                            return;
                        process.OutputDataReceived += (sender, args) => Log($"AZF: {args.Data}");
                        process.ErrorDataReceived += (sender, args) => Log($"AZF: {args.Data}");
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                    });
                })
                .TryOrFailWithGrace(onFail: ex => result = OperationResult.Fail(ex, $"Error occurred while trying to host the Azure Functions App. Message: {ex.Message}")); 

                AppDomain.CurrentDomain.ProcessExit += (sender, args) => {
                    new Action(() => {
                        State.AzureFunctionsHostProcess?.Kill(entireProcessTree: true);
                        State.Clear();
                    }).TryOrFailWithGrace();
                };

                State.AzureFunctionsBaseApiUrl = $"http://localhost:{port}/api";

                return result.AsTask();
            }

            private static string GetCodebaseFolderPath()
            {
                var dllPath = Assembly.GetExecutingAssembly()?.Location ?? string.Empty;
                var srcFolderIndex = dllPath.ToLowerInvariant().IndexOf(srcFolderRelativePath, StringComparison.InvariantCultureIgnoreCase);
                if (srcFolderIndex < 0)
                    return string.Empty;
                var srcFolderPath = Path.GetDirectoryName(dllPath[..(srcFolderIndex + srcFolderRelativePath.Length)]) ?? string.Empty;
                return srcFolderPath;
            }
        }
    }
}
