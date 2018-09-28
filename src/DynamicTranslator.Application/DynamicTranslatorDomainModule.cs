using System.Reflection;

namespace DynamicTranslator.Application
{
    public class DynamicTranslatorDomainModule : DynamicTranslatorModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
