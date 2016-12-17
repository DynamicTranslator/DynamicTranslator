using Abp.Dependency;

using Castle.MicroKernel.Registration;

namespace DynamicTranslator.TestBase
{
    public static class LifeStyleExtensions
    {
        public static ComponentRegistration<T> ApplyLifeStyle<T>(this ComponentRegistration<T> registration, DependencyLifeStyle lifeStyle) where T : class
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Singleton:
                    return registration.LifestyleSingleton();
                case DependencyLifeStyle.Transient:
                    return registration.LifestyleTransient();
                default:
                    return registration;
            }
        }
    }
}
