namespace Dynamic.Translator.Core.Orchestrators
{
    using System;
    using Dependency.Markers;

    public interface ITranslator : ISingletonDependency
    {
        bool IsInitialized { get; set; }

        void Initialize();

        void Dispose();

        void AddNotification(string title, string imageUrl, string message);

        event EventHandler WhenClipboardContainsTextEventHandler;

        event EventHandler<WhenNotificationAddEventArgs> WhenNotificationAddEventHandler;
    }
}