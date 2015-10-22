namespace Dynamic.Translator.Core.Orchestrators
{
    using System;
    using Dependency.Markers;

    public interface ITranslator : ISingletonDependency
    {
        bool IsInitialized { get; set; }

        void Initialize();

        void Dispose();

        void AddNotificationEvent(object sender, WhenNotificationAddEventArgs eventArgs);

        event EventHandler WhenClipboardContainsTextEventHandler;

        event EventHandler<WhenNotificationAddEventArgs> WhenNotificationAddEventHandler;
    }
}