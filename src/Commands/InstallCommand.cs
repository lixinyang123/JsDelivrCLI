using JSDelivrCLI.Common;
using JSDelivrCLI.Models;
using JSDelivrCLI.Services;
using System.CommandLine;

namespace delivr.Commands
{
    internal class InstallCommand : Command
    {
        private readonly ConfigService configService;
        private readonly CDNService cdnService;

        public InstallCommand() : base("install", "install a package from jsdelivr")
        {
            configService = new();
            cdnService = new();

            Argument argument = new("library", "library name and version");
            AddArgument(argument);

            Option versionOption = new("--version", "library version");
            AddOption(versionOption);

            Option dirOption = new("--dir", "library install directory");
            AddOption(dirOption);

            this.SetHandler<string, string, string>(Execute, argument, versionOption, dirOption);
        }

        private async void Execute(string library, string version, string dir)
        {
            ConfigItem item = new(library, version);

            ConfigItem searchItem = configService.GetLibrary(item.Name);
            if (searchItem != null)
            {
                if (Directory.Exists(searchItem.Destination))
                {
                    Directory.Delete(searchItem.Destination, true);
                }

                configService.RemoveLibrary(item.Name);
            }

            bool result = await cdnService.Download(item, dir);
            if (!result)
            {
                ConsoleTool.WriteColorful("\nSome file download faled", ConsoleColor.Red);
                return;
            }

            ConsoleTool.WriteColorful("\nInstall libary successful", ConsoleColor.Green);

            // Add Config
            configService.AddLibrary(new ConfigItem()
            {
                Name = item.Name,
                Version = item.Version,
                Destination = Path.Combine(dir)
            });

            configService.Save();
        }
    }
}
