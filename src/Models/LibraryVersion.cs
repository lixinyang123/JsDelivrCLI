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

    [JsonSerializable(typeof(LibraryVersion))]
    internal partial class LibraryVersionJsonCtx : JsonSerializerContext { }

    public class LibraryTag
    {
        [JsonPropertyName("beta")]
        public string Beta { get; set; }

        [JsonPropertyName("latest")]
        public string Latest { get; set; }
    }

    [JsonSerializable(typeof(LibraryTag))]
    internal partial class LibraryTagJsonCtx : JsonSerializerContext { }
}