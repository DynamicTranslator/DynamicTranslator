namespace DynamicTranslator.LearningStation.Modules
{
    #region using

    using System.IO;
    using System.Text;
    using Nancy;
    using SquishIt.Framework;

    #endregion

    public class BundlesModule : NancyModule
    {
        public BundlesModule() : base("/bundles")
        {
            Get["/js/{name}"] = parameters => CreateResponse(Bundle.JavaScript().RenderCached((string) parameters.name), Configuration.Instance.JavascriptMimeType);
            Get["/css/{name}"] = parameters => CreateResponse(Bundle.Css().RenderCached((string) parameters.name), Configuration.Instance.CssMimeType);
        }

        private Response CreateResponse(string content, string contentType)
        {
            var response = Response.FromStream(() => Stream.Synchronized(new MemoryStream(Encoding.UTF8.GetBytes(content))), contentType);
            if (Request.Query["r"] != null)
            {
                response.WithHeader("etag", (string) Request.Query["r"]);
            }

//            response
//#if debug
//                .WithHeader("Cache-Control", "max-age=45");
//#else
//                .WithHeader("Cache-Control", "max-age=604800");
//#endif
            return response;
        }
    }
}