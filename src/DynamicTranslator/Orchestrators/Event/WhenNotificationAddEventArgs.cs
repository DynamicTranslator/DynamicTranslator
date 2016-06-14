using System;

namespace DynamicTranslator.Orchestrators.Event
{
    public class WhenNotificationAddEventArgs : EventArgs
    {
        public string ImageUrl;

        public string Message;

        public string Title;
    }
}