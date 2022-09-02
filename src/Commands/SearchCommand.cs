using JSDelivrCLI.Models;
using JSDelivrCLI.Services;
using System.CommandLine;
using System.CommandLine.Binding;

namespace delivr.Commands
{
    internal class SearchCommand : Command
    {
        private readonly CDNService cdnService;

        public SearchCommand() : base("search", "search package from npm")
        {
            cdnService = new();

            IValueDescriptor<string> argument = new Argument<string>("library", "search library name");
            AddArgument((Argument)argument);

            this.SetHandler(Execute, argument);
        }

        private void Execute(string library)
        {
            SearchInfo searchInfo = cdnService.Search(library).Result;
            searchInfo.objects.ForEach(obj =>
            {
                Console.WriteLine($"{obj.package.name} => {obj.package.version}");
            });
        }
    }
}
