namespace DynamicTranslator.Core.Dependency.Interceptors
{
    #region using

    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Castle.DynamicProxy;
    using Exception;
    using Helper;
    using Orchestrators;
    using Service.GoogleAnalytics;
    using ViewModel.Constants;

    #endregion

    public class ExceptionInterceptor : IInterceptor
    {
        private readonly IGoogleAnalyticsService googleAnalytics;
        private readonly INotifier notifier;

        public ExceptionInterceptor(INotifier notifier, IGoogleAnalyticsService googleAnalytics)
        {
            this.notifier = notifier;
            this.googleAnalytics = googleAnalytics;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();

                if (AsyncHelper.IsAsyncMethod(invocation.Method))
                {
                    invocation.ReturnValue = ExtendedAsyncHelper.CallAwaitTaskWithFinallyAndGetResult(
                        invocation.Method.ReturnType.GenericTypeArguments[0],
                        invocation.ReturnValue,
                        exception => HandleExceptionAsync(invocation, exception));
                }
            }
            catch (ApiKeyNullException ex)
            {
                if (AsyncHelper.IsAsyncMethod(invocation.Method))
                    HandleExceptionAsync(invocation, ex);
            }
            catch (MaximumCharacterLimitException ex)
            {
                if (AsyncHelper.IsAsyncMethod(invocation.Method))
                    HandleExceptionAsync(invocation, ex);
            }
            catch (WebException ex)
            {
                if (AsyncHelper.IsAsyncMethod(invocation.Method))
                    HandleExceptionAsync(invocation, ex);
            }
            catch (Exception ex)
            {
                HandleException(invocation, ex);
            }
        }

        private void HandleException(IInvocation invocation, Exception ex)
        {
            var exceptionText = new StringBuilder()
                .AppendLine("Exception Occured on:" + invocation.TargetType.Name)
                .AppendLine(ex.Message)
                .AppendLine(ex.InnerException?.Message ?? string.Empty)
                .AppendLine(ex.StackTrace)
                .ToString();

            notifier.AddNotificationAsync(Titles.Exception, ImageUrls.NotificationUrl, exceptionText);

            SendExceptionGoogleAnalyticsAsync(exceptionText, false);
        }

        private void HandleExceptionAsync(IInvocation invocation, Exception ex)
        {
            if (invocation.Method.ReturnType == typeof (void))
                return;

            if (ex == null)
                return;

            var exceptionText = new StringBuilder()
                .AppendLine("Exception Occured on:" + invocation.TargetType.Name)
                .AppendLine(ex.Message)
                .AppendLine(ex.InnerException?.Message ?? string.Empty)
                .AppendLine(ex.StackTrace)
                .ToString();

            SendExceptionGoogleAnalyticsAsync(exceptionText, false);
        }

        private void SendExceptionGoogleAnalyticsAsync(string text, bool isFatal)
        {
            Task.Run(async () => await googleAnalytics.TrackExceptionAsync(text, isFatal));
        }
    }
}