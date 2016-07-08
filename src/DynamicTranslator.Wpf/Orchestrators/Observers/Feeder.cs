using System;
using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Service.GoogleAnalytics;

namespace DynamicTranslator.Wpf.Orchestrators.Observers
{
    public class Feeder : IObserver<long>, ISingletonDependency
    {
        private readonly IGoogleAnalyticsService googleAnalyticsService;

        public Feeder(IGoogleAnalyticsService googleAnalyticsService)
        {
            this.googleAnalyticsService = googleAnalyticsService;
        }

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