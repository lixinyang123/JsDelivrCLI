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

            Argument library = new Argument<string>("library", "library name");
            AddArgument(library);

            Option version = new Option<string>("--version", "library version");
            AddOption(version);

            Option dir = new Option<string>("--dir", "library install directory");
            AddOption(dir);

            this.SetHandler<string, string, string>(Execute, library, version, dir);
        }

        private void Execute(string library, string version, string dir)
        {
            dir ??= string.Empty;
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

            bool result = cdnService.Download(item, dir).Result;
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
