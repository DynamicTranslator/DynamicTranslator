using System;

namespace DynamicTranslator.Domain.Events
{
    public class WhenNotificationAddEventArgs : EventArgs
    {
        public string ImageUrl;

        public string Message;

        public string Title;
    }
}
