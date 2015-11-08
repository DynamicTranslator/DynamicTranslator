namespace Dynamic.Translator.Core.Dependency.Interceptors
{
    using System;
    using Castle.DynamicProxy;
    using Orchestrators;
    using ViewModel.Constants;

    public class ObserverExceptionInterceptor : IInterceptor
    {
        private readonly INotifier notifier;

        public ObserverExceptionInterceptor(INotifier notifier)
        {
            this.notifier = notifier;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                this.notifier.AddNotificationAsync(Titles.Exception, ImageUrls.NotificationUrl, ex.Message);
            }
        }
    }
}