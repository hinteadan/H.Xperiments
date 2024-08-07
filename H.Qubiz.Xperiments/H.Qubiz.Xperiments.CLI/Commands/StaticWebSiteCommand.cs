using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    [Alias("website", "web", "site")]
    internal class StaticWebSiteCommand : CommandBase
    {
        static readonly string[] usageSyntax = [
            "StaticWebSite|website|web|site serve"
        ];
        protected override string[] GetUsageSyntaxes() => usageSyntax;

        public override Task<OperationResult> Run() => RunSubCommand();

        [ID("serve")]
        class ServeSubCommand : SubCommandBase
        {
            private static readonly string srcFolderRelativePath = $"{Path.DirectorySeparatorChar}H.Qubiz.Xperiments{Path.DirectorySeparatorChar}";

            public override async Task<OperationResult> Run(params Note[] args)
            {
                await Task.CompletedTask;
                Log("Serving Static Website...");
                using (new TimeMeasurement(x => Log($"DONE Serving Static Website in {x}")))
                {
                    bool isSameWindow = false;
                    string projectDirPath = Path.Combine(GetCodebaseFolderPath(), "H.Xperiments.AspNetCore.StaticWebSite");

                    OperationResult result = OperationResult.Win();
                    new Action(() =>
                    {
                        State.WebSiteHostProcess = Process.Start(new ProcessStartInfo
                        {
                            Arguments = $"run --project \"{Path.Combine(projectDirPath, "H.Xperiments.AspNetCore.StaticWebSite.csproj")}\" -c Debug",
                            FileName = $"dotnet",
                            WorkingDirectory = projectDirPath,
                            RedirectStandardOutput = isSameWindow,
                            RedirectStandardError = isSameWindow,
                            UseShellExecute = !isSameWindow,
                            CreateNoWindow = isSameWindow,
                            WindowStyle = isSameWindow ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
                        }).And(process => {
                            if (!isSameWindow)
                                return;
                            process.OutputDataReceived += (sender, args) => Log($"WEBSITE: {args.Data}");
                            process.ErrorDataReceived += (sender, args) => Log($"WEBSITE: {args.Data}");
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();
                        });
                    })
                    .TryOrFailWithGrace(onFail: ex => result = OperationResult.Fail(ex, $"Error occurred while trying to host the WebSite. Message: {ex.Message}"));

                    return result;
                }
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


        static class State
        {
            public static Process WebSiteHostProcess = null;
            public static string WebSiteBaseUrl = null;

            public static bool IsRunning => WebSiteHostProcess is not null;

            public static void Clear()
            {
                WebSiteHostProcess = null;
                WebSiteBaseUrl = null;
            }
        }
    }
}
