namespace Dynamic.Translator.Core.Extensions
{
    using Newtonsoft.Json.Linq;

    public static class ObjectExtensions
    {
        public static T GetFirstValueInArrayGraph<T>(this JArray jarray)
        {
            return jarray.ForwardToken().Value<T>();
        }

        private static JToken ForwardToken(this JToken token)
        {
            return token.HasValues ? token.First.ForwardToken() : token;
        }
    }
}