namespace Dynamic.Tureng.Translator.Orchestrator.Finders
{
    using System;
    using System.Threading.Tasks;
    using Observables;

    public interface IMeanFinder
    {
        Task<Maybe<string>> Find(string text);

        event EventHandler<WhenNotificationAddEventArgs> WhenNotificationAddEventHandler;
    }
}