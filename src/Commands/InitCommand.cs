using JSDelivrCLI.Services;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace delivr.Commands
{
    internal class InitCommand : Command
    {
        private readonly ConfigService configService;

        [RequiresDynamicCode("Calls JSDelivrCLI.Services.ConfigService.ConfigService()")]
        public InitCommand() : base("init", "Initialize a package configuration file")
        {
            configService = new();
            this.SetHandler(Execute);
        }

        [RequiresDynamicCode("Calls JSDelivrCLI.Services.ConfigService.Save()")]
        private void Execute()
        {
            configService.Save();
        }
    }
}
