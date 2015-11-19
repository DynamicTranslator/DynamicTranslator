namespace DynamicTranslator.Core.GoogleAnalytics
{
    #region using

    using System.Collections;
    using System.Net;
    using System.Web;
    using Config;

    #endregion

    public class GoogleAnalyticsApi
    {
        private readonly IStartupConfiguration configuration;

        private readonly string googleVersion = "1";
        private readonly string _googleTrackingId = "UA-70082243-2";
        private readonly string _googleClientId = "555";

        public GoogleAnalyticsApi(IStartupConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public GoogleAnalyticsApi(string trackingId, IStartupConfiguration configuration)
        {
            _googleTrackingId = trackingId;
            this.configuration = configuration;
        }

        public void TrackEvent(string category, string action, string label, string value)
        {
            var ht = BaseValues();

            ht.Add("t", "event"); // Event hit type
            ht.Add("ec", category); // Event Category. Required.
            ht.Add("ea", action); // Event Action. Required.
            if (label != null) ht.Add("el", label); // Event label.
            if (value != null) ht.Add("ev", value); // Event value.

            PostData(ht);
        }

        public void TrackPage(string hostname, string page, string title)
        {
            var ht = BaseValues();

            ht.Add("t", "pageview"); // Pageview hit type.
            ht.Add("dh", hostname); // Document hostname.
            ht.Add("dp", page); // Page.
            ht.Add("dt", title); // Title.

            PostData(ht);
        }

        public void EcommerceTransaction(string id, string affiliation, string revenue, string shipping, string tax, string currency)
        {
            var ht = BaseValues();

            ht.Add("t", "transaction"); // Transaction hit type.
            ht.Add("ti", id); // transaction ID.            Required.
            ht.Add("ta", affiliation); // Transaction affiliation.
            ht.Add("tr", revenue); // Transaction revenue.
            ht.Add("ts", shipping); // Transaction shipping.
            ht.Add("tt", tax); // Transaction tax.
            ht.Add("cu", currency); // Currency code.

            PostData(ht);
        }

        public void EcommerceItem(string id, string name, string price, string quantity, string code, string category, string currency)
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

            PostData(ht);
        }

        public void TrackSocial(string action, string network, string target)
        {
            var ht = BaseValues();

            ht.Add("t", "social"); // Social hit type.
            ht.Add("dh", action); // Social Action.         Required.
            ht.Add("dp", network); // Social Network.        Required.
            ht.Add("dt", target); // Social Target.         Required.

            PostData(ht);
        }

        public void TrackException(string description, bool fatal)
        {
            var ht = BaseValues();

            ht.Add("t", "exception"); // Exception hit type.
            ht.Add("dh", description); // Exception description.         Required.
            ht.Add("dp", fatal ? "1" : "0"); // Exception is fatal?            Required.

            PostData(ht);
        }

        private Hashtable BaseValues()
        {
            var ht = new Hashtable();
            ht.Add("v", googleVersion); // Version.
            ht.Add("tid", _googleTrackingId); // Tracking ID / Web property / Property ID.
            ht.Add("cid", _googleClientId); // Anonymous Client ID.
            return ht;
        }

        private bool PostData(IDictionary values)
        {
            var data = "";
            foreach (var key in values.Keys)
            {
                if (data != "") data += "&";
                if (values[key] != null) data += key + "=" + HttpUtility.UrlEncode(values[key].ToString());
            }

            using (var client = new WebClient())
            {
                var result = client.UploadString(configuration.GoogleAnalyticsUrl, "POST", data);
            }

            return true;
        }
    }
}