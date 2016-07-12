using System.Linq;

using Castle.DynamicProxy;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Exceptions;

namespace DynamicTranslator.Dependency.Interceptors
{
    public class TextGuardInterceptor : IInterceptor
    {
        private readonly IApplicationConfiguration configuration;
        private string currentString;

        public TextGuardInterceptor(IApplicationConfiguration configuration)
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

                invocation.Proceed();
            }
        }
    }
}