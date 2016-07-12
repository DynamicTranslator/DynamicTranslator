using System.Reflection;

namespace DynamicTranslator.Domain
{
    public class DynamicTranslatorDomainModule : DynamicTranslatorModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}