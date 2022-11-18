using System.Text.Json.Serialization;

namespace JSDelivrCLI.Models
{
    public class Config
    {
        public Config()
        {
            Libraries = new List<ConfigItem>();
        }

        public List<ConfigItem> Libraries { get; set; }
    }

    [JsonSerializable(typeof(Config))]
    internal partial class ConfigJsonCtx : JsonSerializerContext { }
}