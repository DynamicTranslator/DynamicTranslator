using System.Threading.Tasks;
using DynamicTranslator.Google;

namespace DynamicTranslator
{
    public interface IGoogleAnalyticsTracker
    {
        Task Track();
    }

    public class GoogleAnalyticsTracker : IGoogleAnalyticsTracker
    {
        private readonly IGoogleAnalyticsService _googleAnalyticsService;

        public GoogleAnalyticsTracker(IGoogleAnalyticsService googleAnalyticsService)
        {
            _googleAnalyticsService = googleAnalyticsService;
        }

        public Task Track()
        {
            return _googleAnalyticsService.TrackAppScreenAsync("DynamicTranslator",
                ApplicationVersion.GetCurrentVersion(),
                "dynamictranslator",
                "dynamictranslator",
                "MainWindow");
        }
    }
}