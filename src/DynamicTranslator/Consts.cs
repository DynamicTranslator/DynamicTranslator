namespace DynamicTranslator
{
    public class CacheNames
    {
        public const string MeanCache = "MeanCache";
        public const string ReleaseCache = "ReleaseCache";
    }

    public enum TranslatorType
    {
        Tureng,
        Yandex,
        SesliSozluk,
        Google,
        Bing,
        Prompt,
        Zargan,
        WordReference
    }

    public class Headers
    {
        public const string Accept = "Accept";

        public const string AcceptDefinition =
            "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";

        public const string AcceptEncoding = "Accept-Encoding";
        public const string AcceptEncodingDefinition = "gzip, deflate, sdch";
        public const string AcceptLanguage = "Accept-Language";
        public const string AcceptLanguageDefinition = "en-US,en;q=0.8,tr;q=0.6";
        public const string Ampersand = "&";
        public const string CacheControl = "cache-control";
        public const string ContentType = "Content-Type";
        public const string ContentTypeDefinition = "application/x-www-form-urlencoded";
        public const string NoCache = "no-cache";
        public const string UserAgent = "User-Agent";

        public const string UserAgentDefinition =
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
    }

    public static class ImageUrls
    {
        public const string NotificationUrl = "pack://application:,,,/Resources/notification-icon.png";
    }

    public static class Titles
    {
        public const string Exception = "Exception";
        public const string MaximumLimit = "Maximum Limit";
        public const string Message = "Message";
        public const string StartingMessage = "Starting Message";
        public const string Warning = "Warning";
        public const string Asterix = "*";
    }
}