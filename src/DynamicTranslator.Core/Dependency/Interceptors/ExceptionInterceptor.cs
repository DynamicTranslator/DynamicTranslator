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
    using ViewModel.Constants;

    #endregion

    public class ExceptionInterceptor : IInterceptor
    {
        private readonly INotifier notifier;

        public ExceptionInterceptor(INotifier notifier)
        {
            this.notifier = notifier;
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
                if (AsyncHelper.IsAsyncMethod(invocation.Method))
                {
                    invocation.ReturnValue = HandleReturnAsync(invocation, ex);
                }
                else
                {
                    notifier.AddNotificationAsync(Titles.Exception, ImageUrls.NotificationUrl, new StringBuilder()
                        .AppendLine("Exception Occured on:" + invocation.TargetType.Name)
                        .AppendLine(ex.Message)
                        .AppendLine(ex.InnerException?.Message ?? string.Empty).ToString());
                }
            }
        }

        private dynamic HandleReturnAsync(IInvocation invocation, Exception ex)
        {
            if (invocation.Method.ReturnType == typeof (void))
                return null;

            var retVal = new Task<TranslateResult>(() =>
                new TranslateResult(false,
                    new Maybe<string>(new StringBuilder()
                        .AppendLine("Exception Occured on:" + invocation.TargetType.Name)
                        .AppendLine(ex.Message)
                        .AppendLine(ex.InnerException?.Message ?? string.Empty).ToString())
                    ));

            retVal.Start();

            return retVal;
        }
    }
}