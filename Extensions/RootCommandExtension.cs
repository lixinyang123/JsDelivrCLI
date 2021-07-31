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

        private static void InitService()
        {
            cdnService = new CDNService();
        }

        public static RootCommand Initialize(this RootCommand rootCommand)
        {
            InitService();

            //============================ Initialize Command ==============================
            Command initCommand = new Command("init", "Initialize a package configuration file");
            initCommand.Handler = CommandHandler.Create(() => 
            {
                string configPath = "jsdelivr.json";
                
                if(!File.Exists(configPath))
                    File.WriteAllText(configPath, JsonSerializer.Serialize(new Config()));
            });
            rootCommand.Add(initCommand);

            //============================= Install Command =================================
            Command installCommand = new Command("install", "install a package from jsdelivr");
            installCommand.Add(new Argument<string>("library", "library name and version \n eg. jquery \n eg. jquery@3.6.0"));
            installCommand.Handler = CommandHandler.Create<string>(async (library) => 
            {
                ConfigPara para = new ConfigPara(library);
                string result = await cdnService.Download(para);
                Console.WriteLine(result);
            });
            rootCommand.Add(installCommand);

            //============================== Remove Command ==================================
            Command removeCommand = new Command("remove", "remove a package from local");
            removeCommand.Handler = CommandHandler.Create<string>((packageName) => 
            {
                // Remove client side package
            });
            rootCommand.Add(removeCommand);

            //============================== Search Command ==================================
            Command searchCommand = new Command("search", "search package from npm");
            searchCommand.Handler = CommandHandler.Create<string>((packageName) => 
            {
                // Search package from npm
            });
            rootCommand.Add(searchCommand);

            //============================= Restore Command ==================================
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
