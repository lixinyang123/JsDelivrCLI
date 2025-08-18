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

            Argument<string> libraryArgument = new("library")
            {
                Description = "search library name"
            };
            Add(libraryArgument);

            Option<string> versionOption = new("--version")
            {
                Description = "library version"
            };
            Options.Add(versionOption);

            Option<string> dirOption = new("--dir")
            {
                Description = "library install directory"
            };
            Options.Add(dirOption);

            SetAction(result =>
            {
                string libary = result.GetRequiredValue(libraryArgument);
                string version = result.GetValue(versionOption) ?? string.Empty;
                string dir = result.GetValue(dirOption) ?? string.Empty;
                Execute(libary, version, dir);
            });
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
