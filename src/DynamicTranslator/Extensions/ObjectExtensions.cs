using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace DynamicTranslator.Extensions
{
    #region using

    

    #endregion

    public static class ObjectExtensions
    {
        public static T GetFirstValueInArrayGraph<T>(this JArray jarray)
        {
            return jarray.ForwardToken().Value<T>();
        }

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

        internal static JToken ForwardToken(this JToken token)
        {
            return token.HasValues ? token.First.ForwardToken() : token;
        }
    }
}