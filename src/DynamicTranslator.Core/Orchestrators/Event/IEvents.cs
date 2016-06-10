using System;

namespace DynamicTranslator.Core.Orchestrators.Event
{
    #region using

    

    #endregion

    public interface IEvents
    {
        event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;
    }
}