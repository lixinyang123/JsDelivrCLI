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

            Argument argument = new Argument<string>("library", "search library name");
            AddArgument(argument);

            this.SetHandler<string>(Execute, argument);
        }

        private void Execute(string library)
        {
            LibraryVersion libraryVersion = cdnService.GetLibraryVersions(library).Result;
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
