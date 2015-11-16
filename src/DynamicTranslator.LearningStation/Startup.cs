namespace DynamicTranslator.LearningStation
{
    #region using

    using Owin;

    #endregion

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy(options => { });
        }
    }
}