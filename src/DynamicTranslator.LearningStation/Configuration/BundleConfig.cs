namespace DynamicTranslator.LearningStation.Configuration
{
    #region using

    using SquishIt.Framework;

    #endregion

    public class BundeConfig
    {
        public const string ScriptApp = "app";
        public const string ScriptLibraries = "libs";
        public const string Styles = "styles";

        public static void RegisterBundles()
        {
            RegisterStylesBundles();
            RegisterScriptsBundles();
        }

        private static void RegisterScriptsBundles()
        {
            Bundle.JavaScript()
                .Add("~/App_Core/Framework/scripts/utils/ie10fix.js")
                .Add("~/Scripts/json2.js")
                .Add("~/Scripts/modernizr-2.8.3.js")
                .Add("~/Scripts/jquery-2.1.4.js")
                .Add("~/Scripts/jquery-ui-1.11.4.js")
                .Add("~/Scripts/bootstrap.js")
                .Add("~/Scripts/moment-with-locales.js")
                .Add("~/Scripts/jquery.blockUI.js")
                .Add("~/Scripts/toastr.js")
                .Add("~/Scripts/spin.js")
                .Add("~/Scripts/angular.js")
                .Add("~/Scripts/angular-animate.js")
                .Add("~/Scripts/angular-sanitize.js")
                .Add("~/Scripts/angular-ui-router.js")
                .Add("~/Scripts/angular-ui/ui-bootstrap.js")
                .Add("~/Scripts/angular-ui/ui-bootstrap-tpls.js")
                //.Add("~/Scripts/angular-ui/ui-utils.js")
                .Add("~/App_Core/Framework/scripts/core.js")
                .Add("~/App_Core/Framework/scripts/libs/core.jquery.js")
                .Add("~/App_Core/Framework/scripts/libs/core.toastr.js")
                .Add("~/App_Core/Framework/scripts/libs/core.blockUI.js")
                .Add("~/App_Core/Framework/scripts/libs/core.spin.js")
                .Add("~/App_Core/Framework/scripts/libs/core.sweetalert.js")
                .Add("~/App_Core/Framework/scripts/libs/angularjs/core.ng.js")
                .WithMinifier<SquishIt.Framework.Minifiers.JavaScript.MsMinifier>()
                .AsCached(ScriptLibraries, $"~/bundles/js/{ScriptLibraries}");

            Bundle.JavaScript()
                .AddDirectory("~/App")
                .WithMinifier<SquishIt.Framework.Minifiers.JavaScript.MsMinifier>()
                .AsCached(ScriptApp, $"~/bundles/js/{ScriptApp}");
        }

        private static void RegisterStylesBundles()
        {
            Bundle.Css()
                .Add("~/Content/bootstrap.css")
                .Add("~/Content/toastr.css")
                .WithMinifier<SquishIt.Framework.Minifiers.CSS.YuiMinifier>()
                .AsCached(Styles, $"~/bundles/css/{Styles}");
        }
    }
}