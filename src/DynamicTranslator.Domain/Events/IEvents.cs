using System;

namespace DynamicTranslator.Domain.Events
{
    public interface IDynamicTranslatorEvent
    {
        event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;
    }
}