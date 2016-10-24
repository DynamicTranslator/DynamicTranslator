using System;

namespace DynamicTranslator.Domain.Events
{
    public class WhenClipboardContainsTextEventArgs : EventArgs
    {
        public string CurrentString;
        public bool Handled;
    }
}
