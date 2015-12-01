namespace DynamicTranslator.Core.Orchestrators.Event
{
    #region using

    using System;

    #endregion

    public class WhenClipboardContainsTextEventArgs : EventArgs
    {
        public string CurrentString;
        public bool Handled;
    }
}