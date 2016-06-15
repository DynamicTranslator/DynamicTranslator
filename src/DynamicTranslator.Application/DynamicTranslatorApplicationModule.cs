using System.Reflection;

using Abp.Modules;

using DynamicTranslator.DbReeze;

namespace DynamicTranslator.Application
{
    [DependsOn(typeof(DynamicTranslatorDataModule))]
    public class DynamicTranslatorApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}