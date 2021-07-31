using System.Text.Json.Serialization;

namespace JSDelivrCLI.Models
{
    public class PackageTag
    {
        [JsonPropertyName("beta")]
        public string Beta { get; set; }

        [JsonPropertyName("latest")]
        public string Latest { get; set; }
    }
}