using System;

namespace DynamicTranslator
{
    public class WhenClipboardContainsTextEventArgs : EventArgs
    {
        public string CurrentString { get; set; }
    }
}