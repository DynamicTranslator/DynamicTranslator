namespace DynamicTranslator.Core.Orchestrators.Event
{
    #region using

    using System;

    #endregion

    public interface IEvents
    {
        event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;
    }
}