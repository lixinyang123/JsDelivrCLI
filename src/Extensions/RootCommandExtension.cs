using JSDelivrCLI.Common;
using JSDelivrCLI.Models;
using JSDelivrCLI.Services;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace JSDelivrCLI.Extensions
{
    public static class RootCommandExtension
    {
        private static CDNService cdnService;
        private static ConfigService configService;

        private static void InitService()
        {
            cdnService = new CDNService();
            configService = new ConfigService();
        }

        public static RootCommand Initialize(this RootCommand rootCommand)
        {
            InitService();

            //============================ Initialize Command ==============================
            Command initCommand = new Command("init", "Initialize a package configuration file")
            {
                Handler = CommandHandler.Create(() =>
                {
                    configService.Save();
                })
            };
            rootCommand.Add(initCommand);

            //============================= Install Command =================================
            Command installCommand = new Command("install", "install a package from jsdelivr")
            {
                new Argument<string>("library", "library name and version"),
                new Option<string>("--version", "library version"),
                new Option<string>("--dir", "library install directory")
            };
            installCommand.Handler = CommandHandler.Create<string, string, string>(async (library, version, dir) =>
            {
                ConfigItem item = new ConfigItem(library, version);

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
            });
            rootCommand.Add(installCommand);

            //============================== Remove Command ==================================
            Command removeCommand = new Command("remove", "remove a package from local")
            {
                new Argument<string>("library", "remove library name")
            };
            removeCommand.Handler = CommandHandler.Create<string>(library =>
            {
                ConfigItem item = configService.GetLibrary(library);
                if (item == null)
                {
                    ConsoleTool.WriteColorful("Can't find this library", ConsoleColor.Red);
                    return;
                }

                if (Directory.Exists(item.Destination))
                {
                    Directory.Delete(item.Destination, true);
                }

                configService.RemoveLibrary(item.Name);
                configService.Save();
                ConsoleTool.WriteColorful($"Remove {library} successful", ConsoleColor.Green);
            });
            rootCommand.Add(removeCommand);

            //============================== Search Command ==================================
            Command searchCommand = new Command("search", "search package from npm")
            {
                new Argument<string>("library", "search library name")
            };
            searchCommand.Handler = CommandHandler.Create<string>(async library =>
            {
                SearchInfo searchInfo = await cdnService.Search(library);
                searchInfo.objects.ForEach(obj =>
                {
                    Console.WriteLine($"{obj.package.name} => {obj.package.version}");
                });
            });
            rootCommand.Add(searchCommand);

            //================================ Get Version ======================================
            Command infoCommand = new Command("info", "get library version info")
            {
                new Argument<string>("library", "search library name")
            };
            infoCommand.Handler = CommandHandler.Create<string>(async library =>
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
            });
            rootCommand.Add(infoCommand);

            //============================= Restore Command ==================================
            Command restoreCommand = new Command("restore", "restore client side package")
            {
                Handler = CommandHandler.Create(() =>
                {
                    // Restore client side package
                    configService.GetLibraries().ForEach(library =>
                        {
                            ConfigItem para = new ConfigItem(library.Name, library.Version);
                            bool result = cdnService.Download(para, library.Destination).Result;

                            if (result)
                            {
                                ConsoleTool.WriteColorful($"restore {library.Name} successful\n", ConsoleColor.Green);
                            }
                            else
                            {
                                ConsoleTool.WriteColorful($"restore {library.Name} faled\n", ConsoleColor.Red);
                            }
                        });
                })
            };
            rootCommand.Add(restoreCommand);

            return rootCommand;
        }
    }
}
