using JSDelivrCLI.Common;
using JSDelivrCLI.Models;
using JSDelivrCLI.Services;
using System.CommandLine;

namespace delivr.Commands
{
    internal class InfoCommand : Command
    {
        private readonly CDNService cdnService;

        public InfoCommand() : base("info", "get library version info")
        {
            cdnService = new();

            Argument argument = new("library", "search library name");
            AddArgument(argument);

            this.SetHandler<string>(Execute);
        }

        private async void Execute(string library)
        {
            LibraryVersion libraryVersion = await cdnService.GetLibraryVersions(library);
            libraryVersion.Versions.ForEach(version =>
            {
                if (libraryVersion.Tag.Latest == version)
                {
                    ConsoleTool.WriteColorful($"latest => {version}", ConsoleColor.Green);
                }
                else
                {
                    Console.WriteLine(version);
                }
            });
        }
    }
}
