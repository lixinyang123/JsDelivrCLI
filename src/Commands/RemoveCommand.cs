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

            Argument<string> argument = new("library")
            {
                Description = "remove library name"
            };
            Add(argument);

            SetAction(result =>
            {
                string library = result.GetValue(argument);
                Execute(library);
            });
        }

        private void Execute(string library)
        {
            ConfigItem item = configService.GetLibrary(library);

            if (item == null)
            {
                ConsoleTool.WriteColorful("Can't find this library", ConsoleColor.Red);
                return;
            }

            string libPath = Path.Combine(item.Destination, item.Name);

            if (!string.IsNullOrEmpty(libPath))
            {
                if (Directory.Exists(libPath))
                {
                    Directory.Delete(libPath, true);
                }
            }

            configService.RemoveLibrary(item.Name);
            configService.Save();
            ConsoleTool.WriteColorful($"Remove {library} successful", ConsoleColor.Green);
        }
    }
}
