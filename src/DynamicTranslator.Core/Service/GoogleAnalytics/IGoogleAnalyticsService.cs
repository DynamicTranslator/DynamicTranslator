﻿namespace DynamicTranslator.Core.Service.GoogleAnalytics
{
    #region using

    using System.Threading.Tasks;
    using Dependency.Markers;

    #endregion

    public interface IGoogleAnalyticsService : ITransientDependency
    {
        void TrackEvent(string category, string action, string label, string value);

        Task TrackEventAsync(string category, string action, string label, string value);

        void TrackPage(string hostname, string page, string title);

        Task TrackPageAsync(string hostname, string page, string title);

        void EcommerceTransaction(string id, string affiliation, string revenue, string shipping, string tax, string currency);

        Task EcommerceTransactionAsync(string id, string affiliation, string revenue, string shipping, string tax, string currency);

        void EcommerceItem(string id, string name, string price, string quantity, string code, string category, string currency);

        Task EcommerceItemAsync(string id, string name, string price, string quantity, string code, string category, string currency);

        void TrackSocial(string action, string network, string target);

        Task TrackSocialAsync(string action, string network, string target);

        void TrackException(string description, bool fatal);

        Task TrackExceptionAsync(string description, bool fatal);
    }
}