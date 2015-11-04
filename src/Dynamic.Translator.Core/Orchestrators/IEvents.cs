namespace Dynamic.Translator.Core.Orchestrators
{
    using System;

    public interface IEvents
    {
        event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;
    }
}