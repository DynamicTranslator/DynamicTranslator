using System;

namespace DynamicTranslator.Core
{
    public class WhenClipboardContainsTextEventArgs : EventArgs
    {
        public string CurrentString { get; set; }
    }
}