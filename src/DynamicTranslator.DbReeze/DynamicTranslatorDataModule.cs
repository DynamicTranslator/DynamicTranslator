using System.IO;
using System.Reflection;

using Abp.Modules;

using DynamicTranslator.DbReeze.Configuration.Startup;
using DynamicTranslator.DbReeze.DBReezeNoSQL.Configuration;

namespace DynamicTranslator.DbReeze
{
    [DependsOn(typeof(DynamicTranslatorCoreModule))]
    public class DynamicTranslatorDataModule : AbpModule
    {
        public override void Initialize()
        {
            var noSqlDBPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DynamicTranslatorDb");

            Configuration.Modules.UseDbReeze().WithConfiguration(dbreeze => { dbreeze.Configuration.DBreezeDataFolderName = noSqlDBPath; });

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PreInitialize()
        {
            IocManager.Register<IDbReezeModuleConfiguration, DbReezeModuleConfiguration>();
        }
    }
}