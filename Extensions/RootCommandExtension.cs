using JSDelivrCLI.Common;
using JSDelivrCLI.Models;
using JSDelivrCLI.Services;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Text.Json;

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
            Command initCommand = new Command("init", "Initialize a package configuration file");
            initCommand.Handler = CommandHandler.Create(() => 
            {
                configService.Save();
            });
            rootCommand.Add(initCommand);

            //============================= Install Command =================================
            Command installCommand = new Command("install", "install a package from jsdelivr");
            installCommand.Add(new Argument<string>("library", "library name and version \n eg. jquery \n eg. jquery@3.6.0"));
            installCommand.Add(new Option<string>("--dir", "library install directory"));
            installCommand.Handler = CommandHandler.Create<string, string>((library, dir) => 
            {
                ConfigPara para = new ConfigPara(library);
                bool result = cdnService.Download(para, dir);

                if(!result)
                {
                    ConsoleTool.WriteColorful("\nSome file download faled", ConsoleColor.Red);
                    return;
                }

                ConsoleTool.WriteColorful("\nInstall libary successful", ConsoleColor.Green);
                // Add Config
                configService.AddLibrary(new ConfigItem()
                {
                    Name = para.ToString(),
                    Destination = Path.Combine(dir, para.Name)
                });
                configService.Save();
            });
            rootCommand.Add(installCommand);

            //============================== Remove Command ==================================
            Command removeCommand = new Command("remove", "remove a package from local");
            removeCommand.Add(new Argument<string>("library", "remove library name"));
            removeCommand.Handler = CommandHandler.Create<string>((library) => 
            {
                ConfigItem item = configService.GetLibrary(library);
                if (item ==null || !Directory.Exists(item.Destination))
                {
                    ConsoleTool.WriteColorful("Can't find this library", ConsoleColor.Red);
                    return;
                }
                Directory.Delete(item.Destination, true);
                configService.RemoveLibrary(item.Name);
                configService.Save();
                ConsoleTool.WriteColorful($"Remove {library} successful", ConsoleColor.Green);
            });
            rootCommand.Add(removeCommand);

            //============================== Search Command ==================================
            Command searchCommand = new Command("search", "search package from npm");
            searchCommand.Handler = CommandHandler.Create<string>((libraryName) => 
            {
                // Search package from npm
            });
            rootCommand.Add(searchCommand);

            //============================= Restore Command ==================================
            Command restoreCommand = new Command("restore", "restore client side package");
            restoreCommand.Handler = CommandHandler.Create(() => 
            {
                // Restore client side package
                configService.GetLibraries().ForEach(library => 
                {
                    ConfigPara para = new ConfigPara(library.Name);
                    bool result = cdnService.Download(para, library.Destination.Replace($"{para.Name}", string.Empty));

                    if (result)
                        ConsoleTool.WriteColorful($"restore {library.Name} successful\n", ConsoleColor.Green);
                    else
                        ConsoleTool.WriteColorful($"restore {library.Name} faled\n", ConsoleColor.Red);
                });
            });
            rootCommand.Add(restoreCommand);

            return rootCommand;
        }
    }
}
