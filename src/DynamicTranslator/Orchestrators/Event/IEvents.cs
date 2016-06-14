using System;

namespace DynamicTranslator.Orchestrators.Event
{
    public interface IEvents
    {
        event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;
    }
}