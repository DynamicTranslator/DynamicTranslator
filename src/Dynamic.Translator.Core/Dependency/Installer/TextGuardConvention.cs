namespace Dynamic.Translator.Core.Dependency.Installer
{
    using System.Linq;
    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Facilities;
    using Interceptors;
    using Orchestrators;

    public class TextGuardConvention : AbstractFacility
    {
        protected override void Init()
        {
            this.Kernel.ComponentRegistered += this.KernelOnComponentRegistered;
        }

        private void KernelOnComponentRegistered(string key, IHandler handler)
        {
            var isDefined = handler.ComponentModel.Implementation.GetInterfaces().Contains(typeof (IMeanFinder));


            if (isDefined)
            {
                handler.ComponentModel.Interceptors.AddFirst(new InterceptorReference(typeof (ExceptionInterceptor)));
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof (TextGuardInterceptor)));
            }
        }
    }
}