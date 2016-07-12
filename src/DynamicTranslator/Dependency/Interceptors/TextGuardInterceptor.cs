using System.Linq;

using Castle.DynamicProxy;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Exception;

namespace DynamicTranslator.Dependency.Interceptors
{
    public class TextGuardInterceptor : IInterceptor
    {
        public TextGuardInterceptor(IApplicationConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private readonly IApplicationConfiguration configuration;
        private string currentString;

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Arguments.Any())
            {
                currentString = invocation.Arguments[0].ToString();

                if (currentString.Length > configuration.SearchableCharacterLimit)
                    throw new MaximumCharacterLimitException("You have exceed maximum character limit");

                invocation.Proceed();
            }
        }
    }
}