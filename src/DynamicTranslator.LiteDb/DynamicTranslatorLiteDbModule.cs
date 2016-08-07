using System.IO;
using System.Reflection;

using Abp.Modules;

using DynamicTranslator.LiteDb.Configuration.Startup;
using DynamicTranslator.LiteDb.LiteDb.Configuration;
using DynamicTranslator.LiteDb.LiteDb.Mapper;

namespace DynamicTranslator.LiteDb
{
    [DependsOn(typeof(DynamicTranslatorCoreModule))]
    public class DynamicTranslatorLiteDbModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            var noSqlDbPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DynamicTranslatorDb");

            Configuration.Modules.UseLiteDb().WithConfiguration(cfg => { cfg.Path = noSqlDbPath; });

            LiteDbMap.Initialize();
        }

        public override void PreInitialize()
        {
            IocManager.Register<ILiteDbModuleConfiguration, LiteDbModuleConfiguration>();
        }
    }
}