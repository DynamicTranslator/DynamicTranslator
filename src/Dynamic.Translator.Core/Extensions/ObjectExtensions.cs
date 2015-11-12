namespace Dynamic.Translator.Core.Extensions
{
    #region using

    using Newtonsoft.Json.Linq;

    #endregion

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