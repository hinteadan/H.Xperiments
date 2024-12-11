using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System.Reflection;
using System.Runtime.Loader;

namespace H.Xperiments.Assemblies
{
    [ID("debug")]
    internal class AssemblyDebugSubCommand : SubCommandBase
    {
        public override async Task<OperationResult> Run(params Note[] args)
        {
            Log($"Running {GetType().Name}...");
            using (new TimeMeasurement(x => Log($"DONE Running {GetType().Name} in {x}")))
            {
                AssemblyLoadContext assemblyLoadContextDefault = AssemblyLoadContext.Default;
                AssemblyLoadContext assemblyLoadContextA = new AssemblyLoadContext("A", isCollectible: true);
                AssemblyLoadContext assemblyLoadContextB = new AssemblyLoadContext("B", isCollectible: true);

                //assemblyLoadContextA.LoadFromAssemblyPath(@"C:\Wrk\Playground\H.Xperiments\H.Qubiz.Xperiments\H.Qubiz.Xperiments.RavenDb\bin\Debug\net9.0\H.Qubiz.Xperiments.RavenDb.dll");
                var assembly = assemblyLoadContextA.LoadFromAssemblyName(new AssemblyName("H.Qubiz.Xperiments.RavenDb"));

                var xxx = assemblyLoadContextDefault.LoadFromAssemblyName(new AssemblyName("H.Qubiz.Xperiments.RavenDb"));

                var types = assembly.GetTypes();

                var sharedAssemblies = assemblyLoadContextDefault.Assemblies.ToArray();
                var assembliesA = assemblyLoadContextA.Assemblies.ToArray();
                var assembliesB = assemblyLoadContextB.Assemblies.ToArray();
            }

            return OperationResult.Win();
        }
    }
}
