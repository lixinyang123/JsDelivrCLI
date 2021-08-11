using JSDelivrCLI.Extensions;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace JSDelivrCLI
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            RootCommand rootCommand = new RootCommand("JsDelivr CLI");
            rootCommand.Initialize();
            return rootCommand.Invoke(args);
        }
    }
}
