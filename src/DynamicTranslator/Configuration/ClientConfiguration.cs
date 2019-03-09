namespace DynamicTranslator.Configuration
{
    public class ClientConfiguration
    {
        public string AppVersion { get; set; }

        public WireUp Serviceses { get; }

        public string Id { get; set; }

        public string MachineName { get; set; }
    }
}