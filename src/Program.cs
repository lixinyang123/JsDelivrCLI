using JSDelivrCLI.Extensions;
using System.CommandLine;

namespace JSDelivrCLI
{
    internal class Program
    {
        private static int Main(string[] args) =>
            new RootCommand("JsDelivr CLI")
                .Initialize()
                .Parse(args)
                .Invoke();
    }
}
