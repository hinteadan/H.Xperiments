using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    [Alias("azf")]
    internal class AzureFunctionsCommand : CommandBase
    {
        static readonly string[] usageSyntax = [
            "azf|azurefunctions start|run|run-host|host|runhost [port=8898] [verbose]",
            "azf|azurefunctions stop|end|kill|stop-host|stophost",
        ];
        protected override string[] GetUsageSyntaxes() => usageSyntax;

        public override Task<OperationResult> Run() => RunSubCommand();


        [Alias("stop", "end", "kill", "stop-host")]
        class StopHostSubCommand : SubCommandBase
        {
            public override Task<OperationResult> Run(params Note[] args)
            {
                throw new NotImplementedException();
            }
        }

        [Alias("start", "run", "run-host", "host")]
        class RunHostSubCommand : SubCommandBase
        {
            private static readonly string srcFolderRelativePath = $"{Path.DirectorySeparatorChar}H.Qubiz.Xperiments{Path.DirectorySeparatorChar}";
            public override Task<OperationResult> Run(params Note[] args)
            {
                string port = args?.Get("port", ignoreCase: true);
                port ??= "7277";

                bool isVerbose = args?.Any(a => a.ID.Is("verbose")) == true;

                string projectDirPath = Path.Combine(GetCodebaseFolderPath(), "H.Xperiments.Azf.Runtime.Debug");

                Process azureFunctionsHostProcess = null;
                OperationResult result = OperationResult.Win();
                new Action(() =>
                {
                    azureFunctionsHostProcess = Process.Start(new ProcessStartInfo
                    {
                        Arguments = $"start --port {port}{(isVerbose ? " --verbose" : "")}",
                        FileName = $"func",
                        WorkingDirectory = projectDirPath,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                    }).And(process => {
                        process.OutputDataReceived += (sender, args) => Log($"AZF: {args.Data}");
                        process.ErrorDataReceived += (sender, args) => Log($"AZF: {args.Data}");
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                    });
                })
                .TryOrFailWithGrace(onFail: ex => result = OperationResult.Fail(ex, $"Error occurred while trying to host the Azure Functions App. Message: {ex.Message}")); 

                AppDomain.CurrentDomain.ProcessExit += (sender, args) => {
                    new Action(() => azureFunctionsHostProcess.Dispose())
                    .TryOrFailWithGrace(onFail: ex => azureFunctionsHostProcess.Kill(entireProcessTree: true));
                };

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
