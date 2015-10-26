namespace Dynamic.Translator.Core.Orchestrators
{
    using System;

    public class WhenClipboardContainsTextEventArgs : EventArgs
    {
        public string CurrentString;
        public bool Handled;
    }
}