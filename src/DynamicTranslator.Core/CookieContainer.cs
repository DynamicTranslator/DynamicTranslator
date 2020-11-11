namespace DynamicTranslator.Core
{
    using System.Collections.Concurrent;
    using System.Net;

    public class CookieContainer
    {
        readonly ConcurrentDictionary<TranslatorType, Cookie> cookies =
            new ConcurrentDictionary<TranslatorType, Cookie>();

        public void Save(TranslatorType translator, Cookie c)
        {
            this.cookies.TryAdd(translator, c);
        }
    }
}