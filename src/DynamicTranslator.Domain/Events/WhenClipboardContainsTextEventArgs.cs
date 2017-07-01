using System;

namespace DynamicTranslator.Domain.Events
{
    public class WhenClipboardContainsTextEventArgs : EventArgs
    {
        public string CurrentString { get; set; }

        public bool Handled { get; set; }
    }
}
