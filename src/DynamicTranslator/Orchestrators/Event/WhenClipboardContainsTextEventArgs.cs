using System;

namespace DynamicTranslator.Orchestrators.Event
{
    #region using

    

    #endregion

    public class WhenClipboardContainsTextEventArgs : EventArgs
    {
        public string CurrentString;
        public bool Handled;
    }
}