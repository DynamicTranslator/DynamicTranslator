using System;

namespace DynamicTranslator.Core.Orchestrators.Event
{
    #region using

    

    #endregion

    public class WhenClipboardContainsTextEventArgs : EventArgs
    {
        public string CurrentString;
        public bool Handled;
    }
}