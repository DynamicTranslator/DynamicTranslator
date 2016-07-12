namespace DynamicTranslator.Configuration.Startup
{
    public class TranslatorModuleConfigurations : ITranslatorModuleConfigurations
    {
        public TranslatorModuleConfigurations(IDynamicTranslatorConfiguration configurations)
        {
            Configurations = configurations;
        }

        public IDynamicTranslatorConfiguration Configurations { get; }
    }
}