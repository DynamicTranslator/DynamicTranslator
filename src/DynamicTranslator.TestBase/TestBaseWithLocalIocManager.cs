using System;

using Abp.Dependency;

using Castle.MicroKernel.Registration;

namespace DynamicTranslator.TestBase
{
    public class TestBaseWithLocalIocManager
    {
        protected TestBaseWithLocalIocManager()
        {
            LocalIocManager = new IocManager();
        }

        protected IIocManager LocalIocManager { get; }

        protected T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        protected object Resolve(Type typeToResolve)
        {
            return LocalIocManager.Resolve(typeToResolve);
        }

        protected void Register<T>(T instance, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton) where T : class
        {
            LocalIocManager.IocContainer.Register(
                Component.For<T>().Instance(instance).ApplyLifeStyle(lifeStyle)
            );
        }
    }
}
