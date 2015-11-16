namespace DynamicTranslator.Core.Orchestrators
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