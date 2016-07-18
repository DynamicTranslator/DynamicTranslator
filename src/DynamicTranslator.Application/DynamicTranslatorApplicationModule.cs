using System.Reflection;

using Abp.Modules;
using Abp.Runtime.Validation.Interception;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using DynamicTranslator.DbReeze;
using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Application
{
    [DependsOn(typeof(DynamicTranslatorDataModule))]
    public class DynamicTranslatorApplicationModule : DynamicTranslatorModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(
                Component.For<IMeanFinderFactory>().AsFactory(),
                Component.For<IMeanOrganizerFactory>().AsFactory(),
                Component.For<ILanguageDetectorFactory>().AsFactory()
                );

            MethodInvocationValidator.IgnoredTypesForRecursiveValidation.Add(typeof(CompositeTranslateResult));
        }

        public override void PreInitialize()
        {
            IocManager.IocContainer.AddFacility<InterceptorFacility>();
        }
    }
}