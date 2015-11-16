namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using System;

    #endregion

    public interface IEvents
    {
        event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;
    }
}