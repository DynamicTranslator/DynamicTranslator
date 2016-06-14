#region using

using System;
using System.Threading.Tasks;

using DynamicTranslator.Dependency.Markers;
using DynamicTranslator.Service.GoogleAnalytics;
using DynamicTranslator.Wpf.ViewModel.Model;

#endregion

namespace DynamicTranslator.Wpf.Orchestrators.Observers
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