namespace Dynamic.Translator.Core.Orchestrators
{
    using System;
    using Dependency.Markers;

    public interface ITranslator : ISingletonDependency
    {
        void Initialize();

        void WhenNotificationAddEventInvoker(object sender, WhenNotificationAddEventArgs eventArgs);

        event EventHandler WhenClipboardContainsTextEventHandler;
    }
}