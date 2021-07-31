using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JSDelivrCLI.Models
{
    public class Package
    {
        public Package()
        {
            Files = new List<PackageFile>();
        }

        [JsonPropertyName("default")]
        public string DefaultFile { get; set; }

        [JsonPropertyName("files")]
        public List<PackageFile> Files { get; set; }
    }
}