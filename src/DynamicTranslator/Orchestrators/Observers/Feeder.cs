namespace DynamicTranslator.Orchestrators.Observers
{
    #region using

    using System;
    using System.Threading.Tasks;
    using Core.Dependency.Markers;
    using Core.Service.GoogleAnalytics;
    using ViewModel;

    #endregion

    public class Feeder : IObserver<long>, ISingletonDependency
    {
        private readonly IGoogleAnalyticsService googleAnalyticsService;

        public Feeder(IGoogleAnalyticsService googleAnalyticsService)
        {
            this.googleAnalyticsService = googleAnalyticsService;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(long value)
        {
            Task.Run(async () =>
            {
                await googleAnalyticsService.TrackAppScreenAsync("DynamicTranslator",
                    ApplicationVersion.GetCurrentVersion(),
                    "dynamictranslator",
                    "dynamictranslator", "notification");
            });
        }
    }
}