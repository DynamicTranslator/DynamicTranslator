using System;

namespace DynamicTranslator.Application.Events
{
    public interface IDynamicTranslatorEvent
    {
        event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;
    }
}
