using System;
using System.Collections.Generic;

namespace JSDelivrCLI.Models
{
    public class PackageFile
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public DateTime Time { get; set; }
        public int Size { get; set; }
        public List<PackageFile> Files { get; set; }
    }
}