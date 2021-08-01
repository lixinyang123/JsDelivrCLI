using System.Text.Json.Serialization;

namespace JSDelivrCLI.Models
{
    public class LibraryTag
    {
        [JsonPropertyName("beta")]
        public string Beta { get; set; }

        [JsonPropertyName("latest")]
        public string Latest { get; set; }
    }
}