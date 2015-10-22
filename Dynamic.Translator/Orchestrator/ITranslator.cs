namespace Dynamic.Tureng.Translator.Orchestrator
{
    using System;
    using Observables;

    public interface ITranslator
    {
        void Initialize();

        void WhenNotificationAddEventInvoker(object sender, WhenNotificationAddEventArgs eventArgs);

        event EventHandler WhenClipboardContainsTextEventHandler;

    }
}