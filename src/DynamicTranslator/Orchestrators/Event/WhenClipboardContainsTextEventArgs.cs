using System;

namespace DynamicTranslator.Orchestrators.Event
{
    public class WhenClipboardContainsTextEventArgs : EventArgs
    {
        public string CurrentString;
        public bool Handled;
    }
}