namespace DynamicTranslator.Configuration.Startup
{
    public interface ILocalPersistenceConfiguration : IConfiguration
    {
        bool IsEnabled { get; set; }
    }
}