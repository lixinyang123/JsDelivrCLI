using JSDelivrCLI.Common;
using JSDelivrCLI.Models;
using JSDelivrCLI.Services;
using System.CommandLine;
using System.CommandLine.Binding;

namespace delivr.Commands
{
    internal class RemoveCommand : Command
    {
        private readonly ConfigService configService;

        public RemoveCommand() : base("remove", "remove a package from local")
        {
            configService = new();

            IValueDescriptor<string> argument = new Argument<string>("library", "remove library name");
            AddArgument((Argument)argument);

            this.SetHandler(Execute, argument);
        }

        private void Execute(string library)
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
        }
    }
}
