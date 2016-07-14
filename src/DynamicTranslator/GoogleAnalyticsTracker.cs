using System;
using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Service.GoogleAnalytics;

namespace DynamicTranslator
{
    public class GoogleAnalyticsTracker : IObserver<long>, ISingletonDependency
    {
        private readonly IGoogleAnalyticsService googleAnalyticsService;

        public GoogleAnalyticsTracker(IGoogleAnalyticsService googleAnalyticsService)
        {
            this.googleAnalyticsService = googleAnalyticsService;
        }

        public void OnCompleted() {}

        public void OnError(Exception error) {}

        public async void OnNext(long value)
        {
            await Task.Run(async () =>
            {
                await googleAnalyticsService.TrackAppScreenAsync("DynamicTranslator",
                    ApplicationVersion.GetCurrentVersion(),
                    "dynamictranslator",
                    "dynamictranslator",
                    "MainWindow");
            });
        }
    }
}