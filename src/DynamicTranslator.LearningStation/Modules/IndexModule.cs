namespace DynamicTranslator.LearningStation.Modules
{
    #region using

    using Configuration;
    using Nancy;
    using SquishIt.Framework;

    #endregion

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ =>
            {
                var model = new
                {
                    AppRoot = (Request.Url.BasePath ?? "/").TrimStart('/'),
                    Styles = Bundle.Css().RenderNamed(BundeConfig.Styles),
                    Scripts = Bundle.JavaScript().RenderNamed(BundeConfig.ScriptLibraries) +
                              Bundle.JavaScript().RenderNamed(BundeConfig.ScriptApp)
                };
                return View["index",model];
            };
        }
    }
}