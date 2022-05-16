using JSDelivrCLI.Models;
using JSDelivrCLI.Services;
using System.CommandLine;

namespace delivr.Commands
{
    internal class SearchCommand : Command
    {
        private readonly CDNService cdnService;

        public SearchCommand() : base("search", "search package from npm")
        {
            cdnService = new();

            Argument argument = new("library", "search library name");
            AddArgument(argument);

            this.SetHandler<string>(Execute, argument);
        }

        private async void Execute(string library)
        {
            SearchInfo searchInfo = await cdnService.Search(library);
            searchInfo.objects.ForEach(obj =>
            {
                Console.WriteLine($"{obj.package.name} => {obj.package.version}");
            });
        }
    }
}
