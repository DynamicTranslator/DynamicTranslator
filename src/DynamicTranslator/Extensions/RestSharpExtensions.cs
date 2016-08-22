using System.Net;

using RestSharp;

namespace DynamicTranslator.Extensions
{
    public static class RestSharpExtensions
    {
        public static bool Ok(this IRestResponse response)
        {
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}