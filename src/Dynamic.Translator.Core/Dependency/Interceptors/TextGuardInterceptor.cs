namespace Dynamic.Translator.Core.Dependency.Interceptors
{
    using System.Linq;
    using Castle.DynamicProxy;
    using Config;
    using Exception;

    public class TextGuardInterceptor : IInterceptor
    {
        private readonly IStartupConfiguration configuration;
        private string currentString;
        private string previousString;

        public TextGuardInterceptor(IStartupConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Arguments.Any())
            {
                this.currentString = invocation.Arguments[0].ToString();

                if (this.previousString != this.currentString)
                {
                    this.previousString = this.currentString;

                    if (this.currentString.Length > this.configuration.SearchableCharacterLimit)
                    {
                        throw new MaximumCharacterLimitException("You have exceed maximum character limit");
                    }

                    if (!string.IsNullOrEmpty(this.configuration.ApiKey))
                    {
                        invocation.Proceed();
                    }
                    else
                    {
                        throw new ApiKeyNullException("The Api Key cannot be NULL !");
                    }
                }
            }
        }
    }
}