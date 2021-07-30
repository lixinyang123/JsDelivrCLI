using JSDelivrCLI.Extensions;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace JSDelivrCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Startup(args);
        }

        static async void Startup(string[] args)
        {
            RootCommand rootCommand = new RootCommand("JsDelivr CLI");

            // Init SubCommand
            rootCommand.Initialize();

            // Middle Pipeline
            CommandLineBuilder commandLineBuilder = new CommandLineBuilder(rootCommand);
            commandLineBuilder.UseDefaults();
            
            // Build Command
            Parser parser = commandLineBuilder.Build();
            await parser.InvokeAsync(args);
        }
    }
}
