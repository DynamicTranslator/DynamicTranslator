using System.IO;
using System.Reflection;

using Abp.Modules;

using DynamicTranslator.LiteDb.Configuration.Startup;
using DynamicTranslator.LiteDb.LiteDb.Configuration;

namespace DynamicTranslator.LiteDb
{
    [DependsOn(typeof(DynamicTranslatorCoreModule))]
    public class DynamicTranslatorLiteDbModule : AbpModule
    {
        public override void Initialize()
        {
            var noSqlDbPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DynamicTranslatorDb");

            Configuration.Modules.UseLiteDb().WithConfiguration(cfg =>
            {
                cfg.Path = noSqlDbPath;
            });

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PreInitialize()
        {
            IocManager.Register<ILiteDbModuleConfiguration, LiteDbModuleConfiguration>();
        }
    }
}