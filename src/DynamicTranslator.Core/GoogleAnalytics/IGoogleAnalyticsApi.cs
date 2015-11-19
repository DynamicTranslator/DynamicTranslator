namespace DynamicTranslator.Core.GoogleAnalytics
{
    public interface IGoogleAnalyticsApi
    {
        void TrackEvent(string category, string action, string label, string value);

        void TrackPage(string hostname, string page, string title);

        void EcommerceTransaction(string id, string affiliation, string revenue, string shipping, string tax, string currency);

        void EcommerceItem(string id, string name, string price, string quantity, string code, string category, string currency);

        void TrackSocial(string action, string network, string target);

        void TrackException(string description, bool fatal);
    }
}