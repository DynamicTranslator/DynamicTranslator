using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DynamicTranslator.Core.Extensions
{
    #region using

    using Newtonsoft.Json.Linq;

    #endregion

    public static class ObjectExtensions
    {
        public static string ToJsonString(this object obj, bool camelCase = false, bool indented = false)
        {
            var options = new JsonSerializerSettings();

            if (camelCase)
            {
                options.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            if (indented)
            {
                options.Formatting = Formatting.Indented;
            }

            return JsonConvert.SerializeObject(obj, options);
        }

        public static T GetFirstValueInArrayGraph<T>(this JArray jarray)
        {
            return jarray.ForwardToken().Value<T>();
        }

        internal static JToken ForwardToken(this JToken token)
        {
            return token.HasValues ? token.First.ForwardToken() : token;
        }
    }
}