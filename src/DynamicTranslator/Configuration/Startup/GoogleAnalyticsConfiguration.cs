namespace DynamicTranslator.Configuration.Startup
{
    public class GoogleAnalyticsConfiguration : IGoogleAnalyticsConfiguration
    {
        public string TrackingId { get; set; }

        public string Url { get; set; }
    }
}
