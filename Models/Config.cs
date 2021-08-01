using System.Collections.Generic;

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

    public class ConfigItem
    {
        public string Name { get; set; }
        public string Destination { get; set; }
    }
}