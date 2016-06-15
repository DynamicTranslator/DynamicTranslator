using System.IO;
using System.Reflection;

using Abp.Modules;

using DBreeze;

using DynamicTranslator.Extensions;

namespace DynamicTranslator.DbReeze
{
    [DependsOn(typeof(DynamicTranslatorCoreModule))]
    public class DynamicTranslatorDataModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            var noSqlDBPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DynamicTranslatorDb");

            IocManager.Register<DBreezeEngine>(new DBreezeEngine(noSqlDBPath));
        }
    }
}