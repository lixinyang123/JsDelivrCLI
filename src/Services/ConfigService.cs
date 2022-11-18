using JSDelivrCLI.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace JSDelivrCLI.Services
{
    public class ConfigService
    {
        private readonly string configPath = "jsdelivr.json";

        private readonly Config config;

        [RequiresDynamicCode("Calls System.Text.Json.JsonSerializer.Deserialize(string, Type)")]
        public ConfigService()
        {
            if (File.Exists(configPath))
            {
                config = JsonSerializer.Deserialize(File.ReadAllText(configPath), ConfigJsonCtx.Default.Config);
            }
            else
            {
                config = new Config();
            }
        }

        public void AddLibrary(ConfigItem item)
        {
            config.Libraries.Add(item);
        }

        public void RemoveLibrary(string libraryName)
        {
            config.Libraries.Remove(config.Libraries.SingleOrDefault(i => i.Name == libraryName));
        }

        public ConfigItem GetLibrary(string libraryName)
        {
            return config.Libraries.SingleOrDefault(i => i.Name.Contains(libraryName));
        }

        [RequiresDynamicCode("Calls System.Text.Json.JsonSerializer.Serialize(object, Type)")]
        public void Save()
        {
            File.WriteAllText(configPath, JsonSerializer.Serialize(config, ConfigJsonCtx.Default.Config));
        }

        public List<ConfigItem> GetLibraries()
        {
            return config.Libraries;
        }
    }
}