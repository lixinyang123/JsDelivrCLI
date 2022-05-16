using delivr.Commands;
using System.CommandLine;

namespace JSDelivrCLI.Extensions
{
    public static class RootCommandExtension
    {
        public static RootCommand Initialize(this RootCommand rootCommand)
        {
            rootCommand.Add(new InitCommand());
            rootCommand.Add(new InstallCommand());
            rootCommand.Add(new RemoveCommand());
            rootCommand.Add(new SearchCommand());
            rootCommand.Add(new InfoCommand());
            rootCommand.Add(new RestoreCommand());

            return rootCommand;
        }
    }
}
