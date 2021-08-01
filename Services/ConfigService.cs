using JSDelivrCLI.Models;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;

namespace JSDelivrCLI.Services
{
    public class ConfigService
    {
        private readonly string configPath = "jsdelivr.json";

        private readonly Config config;

        public ConfigService()
        {
            if(File.Exists(configPath))
                config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath));
            else
                config = new Config();
        }

        public void AddLibrary(ConfigItem item) => 
            config.Libraries.Add(item);

        public void RemoveLibrary(string libraryName) =>
            config.Libraries.Remove(config.Libraries.SingleOrDefault(i => i.Name == libraryName));

        public ConfigItem GetLibrary(string libraryName) =>
            config.Libraries.SingleOrDefault(i => i.Name.Contains(libraryName));

        public void Save() => 
            File.WriteAllText(configPath,
                JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true })
            );

        public List<ConfigItem> GetLibraries() => config.Libraries;
    }
}