namespace DynamicTranslator.Core
{
    using System;

    public class WhenClipboardContainsTextEventArgs : EventArgs
    {
        public string CurrentString { get; set; }
    }
}