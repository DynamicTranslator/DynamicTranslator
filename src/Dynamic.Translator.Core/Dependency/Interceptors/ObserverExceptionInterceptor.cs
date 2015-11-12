namespace Dynamic.Translator.Core.Dependency.Interceptors
{
    #region using

    using System;
    using Castle.DynamicProxy;
    using Orchestrators;
    using ViewModel.Constants;

    #endregion

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
                notifier.AddNotificationAsync(Titles.Exception, ImageUrls.NotificationUrl, ex.Message);
            }
        }
    }
}