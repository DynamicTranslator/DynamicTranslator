namespace DynamicTranslator.Core.Dependency.Installer
{
    #region using

    using System.Linq;
    using System.Reflection;
    using Castle.Core;
    using Castle.MicroKernel;
    using Domain.Uow;
    using Interceptors;
    using Manager;

    #endregion

    public class UnitOfWorkRegistrar
    {
        public static void Initialize(IocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += KernelOnComponentRegistered;
        }

        private static void KernelOnComponentRegistered(string key, IHandler handler)
        {
            if (UnitOfWorkHelper.IsConventionalUowClass(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.AddFirst(new InterceptorReference(typeof (ExceptionInterceptor)));
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof (UnitOfWorkInterceptor)));
            }
            else if (
                handler.ComponentModel.Implementation.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Any(UnitOfWorkHelper.HasUnitOfWorkAttribute))
            {
                //Intercept all methods of classes those have at least one method that has UnitOfWork attribute.
                //TODO: Intecept only UnitOfWork methods, not other methods!
                handler.ComponentModel.Interceptors.AddFirst(new InterceptorReference(typeof (ExceptionInterceptor)));
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof (UnitOfWorkInterceptor)));
            }
        }
    }
}