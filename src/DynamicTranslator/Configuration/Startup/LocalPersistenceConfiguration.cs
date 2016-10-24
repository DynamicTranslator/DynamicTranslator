namespace DynamicTranslator.Configuration.Startup
{
    public class LocalPersistenceConfiguration : ILocalPersistenceConfiguration
    {
        public LocalPersistenceConfiguration()
        {
            IsEnabled = true;
        }

        public bool IsEnabled { get; set; }
    }
}
