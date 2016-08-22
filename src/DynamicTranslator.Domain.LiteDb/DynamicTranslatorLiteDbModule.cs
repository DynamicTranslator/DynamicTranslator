using System.IO;
using System.Reflection;

using Abp.Modules;

using DynamicTranslator.Domain.LiteDb.Configuration.Startup;
using DynamicTranslator.Domain.LiteDb.LiteDb.Configuration;
using DynamicTranslator.Domain.LiteDb.LiteDb.Mapper;

namespace DynamicTranslator.Domain.LiteDb
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