#region using

using System;
using System.Threading.Tasks;

using DynamicTranslator.Core.Dependency.Markers;
using DynamicTranslator.Core.Service.GoogleAnalytics;
using DynamicTranslator.ViewModel.Model;

#endregion

namespace DynamicTranslator.Orchestrators.Observers
{
    public class Feeder : IObserver<long>, ISingletonDependency
    {
        private readonly IGoogleAnalyticsService googleAnalyticsService;

        public Feeder(IGoogleAnalyticsService googleAnalyticsService)
        {
            if (googleAnalyticsService == null)
                throw new ArgumentNullException(nameof(googleAnalyticsService));

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