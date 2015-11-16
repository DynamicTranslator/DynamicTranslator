namespace DynamicTranslator.Core.Orchestrators
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