namespace Dynamic.Translator.Core.ApplicationKernel.Interceptors
{
    #region using

    using System;
    using Castle.DynamicProxy;

    #endregion

    public class ExceptionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}