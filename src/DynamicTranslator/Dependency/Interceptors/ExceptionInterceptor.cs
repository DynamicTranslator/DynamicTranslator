using System.Net;
using System.Text;
using System.Threading.Tasks;

using Castle.DynamicProxy;

using DynamicTranslator.Exception;
using DynamicTranslator.Helper;
using DynamicTranslator.Service.GoogleAnalytics;

namespace DynamicTranslator.Dependency.Interceptors
{
    public class ExceptionInterceptor : IInterceptor
    {
        public ExceptionInterceptor(IGoogleAnalyticsService googleAnalytics)
        {
            this.googleAnalytics = googleAnalytics;
        }

        private readonly IGoogleAnalyticsService googleAnalytics;

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
            catch (System.Exception ex)
            {
                HandleException(invocation, ex);
            }
        }

        private void HandleException(IInvocation invocation, System.Exception ex)
        {
            var exceptionText = new StringBuilder()
                .AppendLine("Exception Occured on:" + invocation.TargetType.Name)
                .AppendLine(ex.Message)
                .AppendLine(ex.InnerException?.Message ?? string.Empty)
                .AppendLine(ex.StackTrace)
                .ToString();

            SendExceptionGoogleAnalyticsAsync(exceptionText, false);
        }

        private void HandleExceptionAsync(IInvocation invocation, System.Exception ex)
        {
            if (invocation.Method.ReturnType == typeof(void))
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