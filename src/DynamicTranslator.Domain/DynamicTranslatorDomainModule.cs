using System.Reflection;

using Abp.Modules;

namespace DynamicTranslator.Domain
{
    public class DynamicTranslatorDomainModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}