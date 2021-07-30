using System.Collections.Generic;

namespace JSDelivrCLI.Models
{
    class Package
    {
        public string DefaultFile { get; set; }
        public List<PackageFile> Files { get; set; }
    }
}