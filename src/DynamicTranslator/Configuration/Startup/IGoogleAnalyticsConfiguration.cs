namespace DynamicTranslator.Configuration.Startup
{
    public interface IGoogleAnalyticsConfiguration : IMustHaveUrl, IConfiguration
    {
        string TrackingId { get; set; }
    }
}