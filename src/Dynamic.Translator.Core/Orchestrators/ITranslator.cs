namespace Dynamic.Translator.Core.Orchestrators
{
    using System;
    using Dependency.Markers;

    public interface ITranslator : ISingletonDependency, IDisposable
    {
        bool IsInitialized { get; set; }

        void Initialize();

        void AddNotification(string title, string imageUrl, string message);

        event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;

        event EventHandler<WhenNotificationAddEventArgs> WhenNotificationAddEventHandler;
    }
}