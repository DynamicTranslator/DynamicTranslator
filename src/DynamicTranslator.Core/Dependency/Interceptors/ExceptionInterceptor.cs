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
    using Orchestrators.Translate;
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
                    var task = invocation.ReturnValue as Task;
                    if (task != null && task.IsFaulted)
                        invocation.ReturnValue = HandleReturnAsync(invocation, task.Exception);
                }
            }
            catch (ApiKeyNullException ex)
            {
                if (invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof (Task<>))
                    invocation.ReturnValue = HandleReturnAsync(invocation, ex);
            }
            catch (MaximumCharacterLimitException ex)
            {
                if (invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof (Task<>))
                    invocation.ReturnValue = HandleReturnAsync(invocation, ex);
            }
            catch (WebException ex)
            {
                if (invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof (Task<>))
                    invocation.ReturnValue = HandleReturnAsync(invocation, ex);
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

        private dynamic HandleReturnAsync(IInvocation invocation, Exception ex)
        {
            if (invocation.Method.ReturnType == typeof (void))
                return null;

            var exceptionText = new StringBuilder()
                .AppendLine("Exception Occured on:" + invocation.TargetType.Name)
                .AppendLine(ex.Message)
                .AppendLine(ex.InnerException?.Message ?? string.Empty)
                .AppendLine(ex.StackTrace)
                .ToString();

            var retVal = new Task<TranslateResult>(() =>
                new TranslateResult(false,
                    new Maybe<string>(exceptionText)
                    ));

            retVal.Start();

            SendExceptionGoogleAnalyticsAsync(exceptionText, false);

            return retVal;
        }

        private void SendExceptionGoogleAnalyticsAsync(string text, bool isFatal)
        {
            Task.Run(async () => await googleAnalytics.TrackExceptionAsync(text, isFatal));
        }
    }
}