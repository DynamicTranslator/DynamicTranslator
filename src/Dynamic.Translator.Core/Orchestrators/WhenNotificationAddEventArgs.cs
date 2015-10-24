namespace Dynamic.Translator.Core.Orchestrators
{
    using System;

    public class WhenNotificationAddEventArgs : EventArgs
    {
        public string ImageUrl;

        public string Message;

        public string Title;
    }
}