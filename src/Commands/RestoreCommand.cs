using JSDelivrCLI.Common;
using JSDelivrCLI.Models;
using JSDelivrCLI.Services;
using System.CommandLine;

namespace delivr.Commands
{
    internal class RestoreCommand : Command
    {
        private readonly ConfigService configService;
        private readonly CDNService cdnService;


        public RestoreCommand() : base("restore", "restore client side package")
        {
            configService = new();
            cdnService = new();
            this.SetHandler(Execute);
        }

        private void Execute()
        {
            configService.GetLibraries().ForEach(library =>
            {
                ConfigItem para = new(library.Name, library.Version);

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
        }
    }
}
