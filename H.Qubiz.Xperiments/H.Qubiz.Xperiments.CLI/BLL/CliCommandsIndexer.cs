using H.Necessaire;
using H.Necessaire.CLI.Commands;
using H.Necessaire.Runtime.CLI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks.Dataflow;

namespace H.Qubiz.Xperiments.CLI.BLL
{
    internal class CliCommandsIndexer
    {
        static readonly string[] commandTypeNameEndings = ["UseCase", "Command", "CliCommand", "CommandUseCase", "UseCaseCommand", "CliCommandUseCase", "CliUseCaseCommand"];
        static readonly Lazy<CliCommandHelpInfo[]> allKnownCliCommands = new Lazy<CliCommandHelpInfo[]>(IndexAllKnownCliCommands);

        public static CliCommandHelpInfo[] AllKnownCliCommands => allKnownCliCommands.Value;

        static CliCommandHelpInfo[] IndexAllKnownCliCommands()
        {
            Type[] allKnownCommands = typeof(ImACliCommand).GetAllImplementations();

            if (allKnownCommands?.Any() != true)
                return [];

            return
                allKnownCommands
                .Select(Map)
                .ToArray()
                ;
        }

        static CliCommandHelpInfo Map(Type cliCommandConcreteType, int index)
        {
            return
                new CliCommandHelpInfo
                {
                    ConcreteType = cliCommandConcreteType,
                    Name = MapCommandName(cliCommandConcreteType),
                    ID = cliCommandConcreteType.GetID(),
                    Aliases = cliCommandConcreteType.GetAliases(),
                    Categories = cliCommandConcreteType.GetCategories(),
                    UsageSyntaxes = BuildUsageSyntaxes(cliCommandConcreteType),
                };
        }

        static string MapCommandName(Type cliCommandConcreteType)
        {
            string commandName = cliCommandConcreteType.Name;

            foreach (string ending in commandTypeNameEndings)
            {
                if (!commandName.EndsWith(ending, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                commandName = commandName.Substring(0, commandName.Length - ending.Length);

                break;
            }

            return commandName.ToLowerInvariant();
        }

        static string[] BuildUsageSyntaxes(Type cliCommandConcreteType)
        {
            string[] result = null;

            new Action(() => {

                object commandInstance = Activator.CreateInstance(cliCommandConcreteType);
                var method = cliCommandConcreteType.GetRuntimeMethods().Single(x => x.Name == "GetUsageSyntaxes");
                result = method?.Invoke(commandInstance, null) as string[];
                result = result?.Where(x => !x.IsEmpty()).ToArrayNullIfEmpty();

            }).TryOrFailWithGrace();

            if (result?.Any() == true)
                return result;

            //TODO: Try to smartly build usage syntaxes

            return result;
        }
    }
}
