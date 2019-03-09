using System.Collections;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using DynamicTranslator.Configuration;

namespace DynamicTranslator.Google
{
    public interface IGoogleAnalyticsService
    {
        void ECommerceItem(string id, string name, string price, string quantity, string code, string category,
            string currency);

        Task ECommerceItemAsync(string id, string name, string price, string quantity, string code, string category,
            string currency);

        void ECommerceTransaction(string id, string affiliation, string revenue, string shipping, string tax,
            string currency);

        Task ECommerceTransactionAsync(string id, string affiliation, string revenue, string shipping, string tax,
            string currency);

        void TrackAppScreen(string appName, string appVersion, string appId, string appInstallerId, string screenName);

        Task TrackAppScreenAsync(string appName, string appVersion, string appId, string appInstallerId,
            string screenName);

        void TrackEvent(string category, string action, string label, string value);
        Task TrackEventAsync(string category, string action, string label, string value);
        void TrackException(string description, bool fatal);
        Task TrackExceptionAsync(string description, bool fatal);
        void TrackPage(string hostname, string page, string title);
        Task TrackPageAsync(string hostname, string page, string title);
        void TrackSocial(string action, string network, string target);
        Task TrackSocialAsync(string action, string network, string target);
    }

    public class GoogleAnalyticsService : IGoogleAnalyticsService
    {
        private const string GoogleAnalyticsUrl = "http://www.google-analytics.com/collect";
        private const string TrackingId = "UA-70082243-2";
        private readonly ApplicationConfiguration _configuration;
        private const string GoogleVersion = "1";

        public GoogleAnalyticsService(ApplicationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ECommerceItem(string id, string name, string price, string quantity, string code, string category,
            string currency)
        {
            PostData(PrepareECommerceItem(id, name, price, quantity, code, category, currency));
        }

        public Task ECommerceItemAsync(string id, string name, string price, string quantity, string code,
            string category, string currency)
        {
            return PostDataAsync(PrepareECommerceItem(id, name, price, quantity, code, category, currency));
        }

        public void ECommerceTransaction(string id, string affiliation, string revenue, string shipping, string tax,
            string currency)
        {
            PostData(PrepareECommerceTransaction(id, affiliation, revenue, shipping, tax, currency));
        }

        public Task ECommerceTransactionAsync(string id, string affiliation, string revenue, string shipping,
            string tax, string currency)
        {
            return PostDataAsync(PrepareECommerceTransaction(id, affiliation, revenue, shipping, tax, currency));
        }

        public void TrackAppScreen(string appName, string appVersion, string appId, string appInstallerId,
            string screenName)
        {
            PostData(PrepareTrackAppScreen(appName, appVersion, appId, appInstallerId, screenName));
        }

        public Task TrackAppScreenAsync(string appName, string appVersion, string appId, string appInstallerId,
            string screenName)
        {
            return PostDataAsync(PrepareTrackAppScreen(appName, appVersion, appId, appInstallerId, screenName));
        }

        public void TrackEvent(string category, string action, string label, string value)
        {
            PostData(PrepareTrackEvent(category, action, label, value));
        }

        public Task TrackEventAsync(string category, string action, string label, string value)
        {
            return PostDataAsync(PrepareTrackEvent(category, action, label, value));
        }

        public void TrackException(string description, bool fatal)
        {
            PostData(PrepareTrackException(description, fatal));
        }

        public Task TrackExceptionAsync(string description, bool fatal)
        {
            return PostDataAsync(PrepareTrackException(description, fatal));
        }

        public void TrackPage(string hostname, string page, string title)
        {
            PostData(PrepareTrackPage(hostname, page, title));
        }

        public Task TrackPageAsync(string hostname, string page, string title)
        {
            return PostDataAsync(PrepareTrackPage(hostname, page, title));
        }

        public void TrackSocial(string action, string network, string target)
        {
            PostData(PrepareTrackSocial(action, network, target));
        }

        public Task TrackSocialAsync(string action, string network, string target)
        {
            return PostDataAsync(PrepareTrackSocial(action, network, target));
        }

        private Hashtable BaseValues()
        {
            var ht = new Hashtable
            {
                {"v", GoogleVersion},
                {"tid", TrackingId},
                {"cid", _configuration.ClientConfiguration.Id}
            };

            // Version.
            // Tracking ID / Web property / Property ID.
            // Anonymous Client ID.
            return ht;
        }

        private void PostData(IDictionary values)
        {
            var data = "";
            foreach (var key in values.Keys)
            {
                if (data != "")
                {
                    data += "&";
                }

                if (values[key] != null)
                {
                    data += key + "=" + HttpUtility.UrlEncode(values[key].ToString());
                }
            }

            using (var client = new WebClient())
            {
                client.UploadString(GoogleAnalyticsUrl, "POST", data);
            }
        }

        private Task PostDataAsync(IDictionary values)
        {
            var data = "";
            foreach (var key in values.Keys)
            {
                if (data != "")
                {
                    data += "&";
                }

                if (values[key] != null)
                {
                    data += key + "=" + HttpUtility.UrlEncode(values[key].ToString());
                }
            }

            using (var client = new WebClient())
            {
                return client.UploadStringTaskAsync(GoogleAnalyticsUrl, "POST", data);
            }
        }

        private Hashtable PrepareECommerceItem(string id, string name, string price, string quantity, string code,
            string category, string currency)
        {
            var ht = BaseValues();

            ht.Add("t", "item"); // Item hit type.
            ht.Add("ti", id); // transaction ID.            Required.
            ht.Add("in", name); // Item name.                 Required.
            ht.Add("ip", price); // Item price.
            ht.Add("iq", quantity); // Item quantity.
            ht.Add("ic", code); // Item code / SKU.
            ht.Add("iv", category); // Item variation / category.
            ht.Add("cu", currency); // Currency code.

            return ht;
        }

        private Hashtable PrepareECommerceTransaction(string id, string affiliation, string revenue, string shipping,
            string tax, string currency)
        {
            var ht = BaseValues();

            ht.Add("t", "transaction"); // Transaction hit type.
            ht.Add("ti", id); // transaction ID.            Required.
            ht.Add("ta", affiliation); // Transaction affiliation.
            ht.Add("tr", revenue); // Transaction revenue.
            ht.Add("ts", shipping); // Transaction shipping.
            ht.Add("tt", tax); // Transaction tax.
            ht.Add("cu", currency); // Currency code.

            return ht;
        }

        private Hashtable PrepareTrackAppScreen(string appName, string appVersion, string appId, string appInstallerId,
            string screenName)
        {
            var ht = BaseValues();

            ht.Add("t", "screenview"); // Pageview hit type.
            ht.Add("an", appName); //App Name
            ht.Add("av", appVersion); //App version.
            ht.Add("aid", appId); //App Id.
            ht.Add("aiid", appInstallerId); //App Installer Id.
            ht.Add("cd", screenName); //Screen name / content description.

            return ht;
        }

        private Hashtable PrepareTrackEvent(string category, string action, string label, string value)
        {
            var ht = BaseValues();

            ht.Add("t", "event"); // Event hit type
            ht.Add("ec", category); // Event Category. Required.
            ht.Add("ea", action); // Event Action. Required.
            if (label != null)
            {
                ht.Add("el", label); // Event label.
            }

            if (value != null)
            {
                ht.Add("ev", value); // Event value.
            }

            return ht;
        }

        private Hashtable PrepareTrackException(string description, bool fatal)
        {
            var ht = BaseValues();

            ht.Add("t", "exception"); // Exception hit type.
            ht.Add("exd", description); // Exception description.         Required.
            ht.Add("exf", fatal ? "1" : "0"); // Exception is fatal?            Required.

            return ht;
        }

        private Hashtable PrepareTrackPage(string hostname, string page, string title)
        {
            var ht = BaseValues();

            ht.Add("t", "pageview"); // Pageview hit type.
            ht.Add("dh", hostname); // Document hostname.
            ht.Add("dp", page); // Page.
            ht.Add("dt", title); // Title.

            return ht;
        }

        private Hashtable PrepareTrackSocial(string action, string network, string target)
        {
            var ht = BaseValues();

            ht.Add("t", "social"); // Social hit type.
            ht.Add("dh", action); // Social Action.         Required.
            ht.Add("dp", network); // Social Network.        Required.
            ht.Add("dt", target); // Social Target.         Required.

            return ht;
        }
    }
}