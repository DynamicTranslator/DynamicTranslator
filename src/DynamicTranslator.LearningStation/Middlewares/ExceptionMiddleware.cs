namespace DynamicTranslator.LearningStation.Middlewares
{
    #region using

    using System;
    using System.Threading.Tasks;
    using Microsoft.Owin;

    #endregion

    public class ExceptionMiddleware : OwinMiddleware
    {
        public ExceptionMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception ex)
            {
                throw new Exception("Owin Exception", ex);
            }
        }
    }
}