using System;
using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Service.GoogleAnalytics;

namespace DynamicTranslator.Wpf.Observers
{
    public class Feeder : IObserver<long>, ISingletonDependency
    {
        public Feeder(IGoogleAnalyticsService googleAnalyticsService)
        {
            this.googleAnalyticsService = googleAnalyticsService;
        }

        private readonly IGoogleAnalyticsService googleAnalyticsService;

        public void OnCompleted() {}

        public void OnError(System.Exception error) {}

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