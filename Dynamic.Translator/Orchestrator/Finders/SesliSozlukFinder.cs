namespace Dynamic.Tureng.Translator.Orchestrator.Finders
{
    using System;
    using System.Threading.Tasks;
    using Observables;

    public class SesliSozlukFinder :IMeanFinder
    {
        public Task<Maybe<string>> Find(string text)
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler<WhenNotificationAddEventArgs> WhenNotificationAddEventHandler;
    }
}