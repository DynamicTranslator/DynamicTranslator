using System;

namespace DynamicTranslator.Orchestrators.Event
{
    #region using

    

    #endregion

    public interface IEvents
    {
        event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;
    }
}