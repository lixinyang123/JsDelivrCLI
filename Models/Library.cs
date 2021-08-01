using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JSDelivrCLI.Models
{
    public class Library
    {
        public Library()
        {
            Files = new List<LibraryFile>();
        }

        [JsonPropertyName("default")]
        public string DefaultFile { get; set; }

        [JsonPropertyName("files")]
        public List<LibraryFile> Files { get; set; }
    }
}