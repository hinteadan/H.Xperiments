using H.Necessaire;
using H.Necessaire.CLI.Commands;
using H.Necessaire.Runtime.CLI.Commands;
using System;
using System.Linq;
using System.Reflection;

namespace H.Qubiz.Xperiments.CLI.BLL
{
    internal class CliCommandsIndexer
    {
        static readonly string[] commandTypeNameEndings = ["UseCase", "Command", "CliCommand", "CommandUseCase", "UseCaseCommand", "CliCommandUseCase", "CliUseCaseCommand"];
        static readonly MethodInfo methodInfo = typeof(CommandBase).GetMethod("GetUsageSyntaxes");
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

            return commandName;
        }
    }

    internal class CliCommandHelpInfo
    {
        public Type ConcreteType { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string[] Aliases { get; set; }
        public string[] Categories { get; set; }

        public string GetPreferredCommandSyntax()
        {
            if (!ID.IsEmpty())
                return ID;

            if (Aliases?.Any(a => !a.IsEmpty()) == true)
                return Aliases.First(a => !a.IsEmpty());

            if (!Name.IsEmpty())
                return Name;

            return ConcreteType.Name;
        }

        public override string ToString()
        {
            return $"{GetPreferredCommandSyntax()} [{ConcreteType.TypeName()}]";
        }
    }
}
