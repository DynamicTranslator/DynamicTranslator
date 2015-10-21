namespace Dynamic.Tureng.Translator.Orchestrator
{
    using System;
    using Observable;

    public interface ITranslator
    {
        void Initialize();

        void WhenNotificationAddEventInvoker(object sender, WhenNotificationAddEventArgs eventArgs);

        event EventHandler WhenClipboardContainsTextEventHandler;

        event EventHandler<WhenNotificationAddEventArgs> WhenNotificationAddEventHandler;
    }
}