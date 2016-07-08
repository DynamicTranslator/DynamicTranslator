namespace DynamicTranslator.Configuration
{
    public interface IAppConfigManager
    {
        string Get(string key);

        IAppConfigManager SaveOrUpdate(string key, string value);
    }
}