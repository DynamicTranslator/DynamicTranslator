using System;

namespace DynamicTranslator.Application.Events
{
    public class WhenNotificationAddEventArgs : EventArgs
    {
        public string ImageUrl;

        public string Message;

        public string Title;
    }
}
