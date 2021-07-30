using System.CommandLine;
using System.CommandLine.Invocation;

namespace JSDelivrCLI.Extensions
{
    public static class RootCommandExtension
    {
        public static RootCommand Initialize(this RootCommand rootCommand)
        {
            // Install Command
            Command installCommand = new Command("install", "install a package from jsdelivr");
            installCommand.Handler = CommandHandler.Create<string>((packageName) => 
            {
                // Install client side package
            });
            rootCommand.Add(installCommand);

            // Remove Command
            Command removeCommand = new Command("remove", "remove a package from local");
            removeCommand.Handler = CommandHandler.Create<string>((packageName) => 
            {
                // Remove client side package
            });
            rootCommand.Add(removeCommand);

            // Search Command
            Command searchCommand = new Command("search", "search package from npm");
            searchCommand.Handler = CommandHandler.Create<string>((packageName) => 
            {
                // Search package from npm
            });
            rootCommand.Add(searchCommand);

            // Restore Command
            Command restoreCommand = new Command("restore", "restore client side package");
            restoreCommand.Handler = CommandHandler.Create(() => 
            {
                // Restore client side package
            });
            rootCommand.Add(restoreCommand);

            return rootCommand;
        }
    }
}
