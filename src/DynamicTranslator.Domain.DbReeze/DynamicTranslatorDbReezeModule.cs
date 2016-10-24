using System.IO;
using System.Reflection;

using Abp.Modules;

using DynamicTranslator.Domain.DbReeze.Configuration.Startup;
using DynamicTranslator.Domain.DbReeze.DBReezeNoSQL.Configuration;

namespace DynamicTranslator.Domain.DbReeze
{
    [DependsOn(typeof(DynamicTranslatorCoreModule))]
    public class DynamicTranslatorDbReezeModule : DynamicTranslatorModule
    {
        public override void Initialize()
        {
            string noSqlDbPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DynamicTranslatorDb");

            Configuration.Modules.UseDbReeze().WithConfigurations(dbreeze => { dbreeze.Configuration.DBreezeDataFolderName = noSqlDbPath; });

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PreInitialize()
        {
            IocManager.Register<IDbReezeModuleConfiguration, DbReezeModuleConfiguration>();
        }
    }
}
