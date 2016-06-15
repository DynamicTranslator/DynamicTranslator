using System.Threading.Tasks;

using Abp.Dependency;

namespace DynamicTranslator.Service.GoogleAnalytics
{
    public interface IGoogleAnalyticsService : ITransientDependency
    {
        void EcommerceItem(string id, string name, string price, string quantity, string code, string category, string currency);

        Task EcommerceItemAsync(string id, string name, string price, string quantity, string code, string category, string currency);

        void EcommerceTransaction(string id, string affiliation, string revenue, string shipping, string tax, string currency);

        Task EcommerceTransactionAsync(string id, string affiliation, string revenue, string shipping, string tax, string currency);

        void TrackAppScreen(string appName, string appVersion, string appId, string appInstallerId, string screenName);

        Task TrackAppScreenAsync(string appName, string appVersion, string appId, string appInstallerId, string screenName);

        void TrackEvent(string category, string action, string label, string value);

        Task TrackEventAsync(string category, string action, string label, string value);

        void TrackException(string description, bool fatal);

        Task TrackExceptionAsync(string description, bool fatal);

        void TrackPage(string hostname, string page, string title);

        Task TrackPageAsync(string hostname, string page, string title);

        void TrackSocial(string action, string network, string target);

        Task TrackSocialAsync(string action, string network, string target);
    }
}