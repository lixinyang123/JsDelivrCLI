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

    public class LibraryFile
    {
        public LibraryFile()
        {
            Files = new List<LibraryFile>();
        }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("files")]
        public List<LibraryFile> Files { get; set; }
    }
}