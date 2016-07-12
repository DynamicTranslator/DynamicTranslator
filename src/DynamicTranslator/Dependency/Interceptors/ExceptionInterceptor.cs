using System.Net;
using System.Text;
using System.Threading.Tasks;

using Castle.DynamicProxy;

using DynamicTranslator.Exception;
using DynamicTranslator.Helper;
using DynamicTranslator.Service.GoogleAnalytics;

using Newtonsoft.Json;

namespace DynamicTranslator.Dependency.Interceptors
{
    public class ExceptionInterceptor : IInterceptor
    {
        private readonly IGoogleAnalyticsService googleAnalytics;

        public ExceptionInterceptor(IGoogleAnalyticsService googleAnalytics)
        {
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
                        async exception => await HandleExceptionAsync(invocation, exception));
                }
            }
            catch (ApiKeyNullException ex)
            {
                if (AsyncHelper.IsAsyncMethod(invocation.Method))
                {
                    HandleExceptionAsync(invocation, ex);
                }
                else
                {
                    HandleException(invocation, ex);
                }
            }
            catch (MaximumCharacterLimitException ex)
            {
                if (AsyncHelper.IsAsyncMethod(invocation.Method))
                {
                    HandleExceptionAsync(invocation, ex);
                }
                else
                {
                    HandleException(invocation, ex);
                }
            }
            catch (WebException ex)
            {
                if (AsyncHelper.IsAsyncMethod(invocation.Method))
                {
                    HandleExceptionAsync(invocation, ex);
                }
                else
                {
                    HandleException(invocation, ex);
                }
            }
            catch (JsonReaderException ex)
            {
                if (AsyncHelper.IsAsyncMethod(invocation.Method))
                {
                    HandleExceptionAsync(invocation, ex);
                }
                else
                {
                    HandleException(invocation, ex);
                }
            }
            catch (System.Exception ex)
            {
                if (AsyncHelper.IsAsyncMethod(invocation.Method))
                {
                    HandleExceptionAsync(invocation, ex);
                }
                else
                {
                    HandleException(invocation, ex);
                }
            }
        }

        private static string ExtractExceptionMessage(IInvocation invocation, System.Exception ex)
        {
            return new StringBuilder()
                .AppendLine("Exception Occured on:" + invocation.TargetType.Name)
                .AppendLine(ex.Message)
                .AppendLine(ex.InnerException?.Message ?? string.Empty)
                .AppendLine(ex.StackTrace)
                .ToString();
        }

        private void HandleException(IInvocation invocation, System.Exception ex)
        {
            if (ex == null)
                return;

            var exceptionText = ExtractExceptionMessage(invocation, ex);

            SendExceptionGoogleAnalytics(exceptionText, false);
        }

        private Task HandleExceptionAsync(IInvocation invocation, System.Exception ex)
        {
            if (ex == null)
                return Task.FromResult(0);

            var exceptionText = ExtractExceptionMessage(invocation, ex);

            return SendExceptionGoogleAnalyticsAsync(exceptionText, false);
        }

        private void SendExceptionGoogleAnalytics(string text, bool isFatal)
        {
            googleAnalytics.TrackException(text, isFatal);
        }

        private Task SendExceptionGoogleAnalyticsAsync(string text, bool isFatal)
        {
            return googleAnalytics.TrackExceptionAsync(text, isFatal);
        }
    }
}