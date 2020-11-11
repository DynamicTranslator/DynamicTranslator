namespace DynamicTranslator.Core
{
    using System.Threading.Tasks;
    using Google;

    public interface IGoogleAnalyticsTracker
    {
        Task Track();
    }

    public class GoogleAnalyticsTracker : IGoogleAnalyticsTracker
    {
        readonly IGoogleAnalyticsService googleAnalyticsService;

        public GoogleAnalyticsTracker(IGoogleAnalyticsService googleAnalyticsService)
        {
            this.googleAnalyticsService = googleAnalyticsService;
        }

        public Task Track()
        {
            return this.googleAnalyticsService.TrackAppScreenAsync("DynamicTranslator",
                ApplicationVersion.GetCurrentVersion(),
                "dynamictranslator",
                "dynamictranslator",
                "MainWindow");
        }
    }
}