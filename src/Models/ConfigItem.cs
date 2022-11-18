using System.Text.Json.Serialization;

namespace JSDelivrCLI.Models
{
    public class ConfigItem
    {
        public ConfigItem() { }

        public ConfigItem(string name, string version, string destination = "")
        {
            Name = name;
            Version = version;
            Destination = destination;
        }

        public string Name { get; set; }
        public string Version { get; set; }
        public string Destination { get; set; }

        public override string ToString()
        {
            return $"{Name}@{Version}";
        }
    }

    [JsonSerializable(typeof(ConfigItem))]
    internal partial class ConfigItemJsonCtx : JsonSerializerContext { }
}