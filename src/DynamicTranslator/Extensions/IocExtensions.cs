using System;

using Abp.Dependency;

using Castle.MicroKernel.Registration;

namespace DynamicTranslator.Extensions
{
    public static class IocExtensions
    {
        public static void Register<TType>(this IIocManager iocManager, object instance, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            Register(iocManager, typeof(TType), instance, lifeStyle);
        }

        public static void Register(this IIocManager iocManager, Type serviceType, object instance, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            iocManager.IocContainer.Register(
                Component.For(serviceType).Instance(instance).ApplyLifestyle(lifeStyle)
                );
        }

        private static ComponentRegistration<T> ApplyLifestyle<T>(this ComponentRegistration<T> registration, DependencyLifeStyle lifeStyle)
            where T : class
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Transient:
                    return registration.LifestyleTransient();
                case DependencyLifeStyle.Singleton:
                    return registration.LifestyleSingleton();
                default:
                    return registration;
            }
        }
    }
}