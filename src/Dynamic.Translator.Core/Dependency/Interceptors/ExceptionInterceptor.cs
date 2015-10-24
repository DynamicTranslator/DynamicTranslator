namespace Dynamic.Translator.Core.Dependency.Interceptors
{
    using System;
    using System.Threading.Tasks;
    using Castle.DynamicProxy;
    using Exception;
    using Orchestrators;

    public class ExceptionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (ApiKeyNullException ex)
            {
                if (invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof (Task<>))
                {
                    invocation.ReturnValue = this.HandleReturnAsync(invocation, ex);
                }
            }
            catch (MaximumCharacterLimitException ex)
            {
                if (invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof (Task<>))
                {
                    invocation.ReturnValue = this.HandleReturnAsync(invocation, ex);
                }
            }
        }

        private dynamic HandleReturnAsync(IInvocation invocation, Exception ex)
        {
            if (invocation.Method.ReturnType == typeof (void))
                return null;

            var retVal = (new Task<TranslateResult>(() => new TranslateResult(false, new Maybe<string>(ex.Message))));
            retVal.Start();

            return retVal;
        }
    }
}