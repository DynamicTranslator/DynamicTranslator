namespace DynamicTranslator.Core.Dependency.Installer
{
    #region using

    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Facilities;
    using Interceptors;
    using Service.GoogleAnalytics;

    #endregion

    public class GoogleAnalyticsConvention : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.ComponentRegistered += KernelOnComponentRegistered;
        }

        private void KernelOnComponentRegistered(string key, IHandler handler)
        {
            if (typeof (IGoogleAnalyticsService).IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.AddFirst(new InterceptorReference(typeof (ExceptionInterceptor)));
            }
        }
    }
}