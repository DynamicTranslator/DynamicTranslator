using System.Linq;

using Castle.DynamicProxy;

using DynamicTranslator.Config;
using DynamicTranslator.Exception;

namespace DynamicTranslator.Dependency.Interceptors
{
    #region using

    

    #endregion

    public class TextGuardInterceptor : IInterceptor
    {
        private readonly IStartupConfiguration configuration;
        private string currentString;

        public TextGuardInterceptor(IStartupConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Arguments.Any())
            {
                currentString = invocation.Arguments[0].ToString();

                if (currentString.Length > configuration.SearchableCharacterLimit)
                    throw new MaximumCharacterLimitException("You have exceed maximum character limit");

                if (string.IsNullOrEmpty(configuration.ApiKey))
                    throw new ApiKeyNullException("The Api Key cannot be NULL !");

                invocation.Proceed();
            }
        }
    }
}