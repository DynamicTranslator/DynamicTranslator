namespace DynamicTranslator.Configuration.Startup
{
    public interface ITranslatorModuleConfigurations : IConfiguration
    {
        IDynamicTranslatorConfiguration Configurations { get; }
    }
}
