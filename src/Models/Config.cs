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
}