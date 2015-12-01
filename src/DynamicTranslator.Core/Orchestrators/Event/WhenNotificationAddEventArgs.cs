namespace DynamicTranslator.Core.Orchestrators.Event
{
    #region using

    using System;

    #endregion

    public class WhenNotificationAddEventArgs : EventArgs
    {
        public string ImageUrl;

        public string Message;

        public string Title;
    }
}