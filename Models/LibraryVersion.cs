using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JSDelivrCLI.Models
{
    public class LibraryVersion
    {
        public LibraryVersion()
        {
            Versions = new List<string>();
        }

        [JsonPropertyName("tags")]
        public LibraryTag Tag { get; set; }

        [JsonPropertyName("versions")]
        public List<string> Versions { get; set; }
    }
}