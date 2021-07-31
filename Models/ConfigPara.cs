namespace JSDelivrCLI.Models
{
    public class ConfigPara
    {
        public ConfigPara(string para)
        {
            string[] paras = para.Split("@");
            Name = paras[0];
            if(paras.Length > 1)
                Version = paras[1];
        }

        public string Name { get; set; }

        public string Version { get; set; }

        public override string ToString()
        {
            return $"{Name}@{Version}";
        }
    }
}