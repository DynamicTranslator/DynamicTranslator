using System;

namespace DynamicTranslator.Application.Events
{
    public class WhenClipboardContainsTextEventArgs : EventArgs
    {
        public string CurrentString { get; set; }
    }
}
