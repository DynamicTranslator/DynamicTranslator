namespace Dynamic.Tureng.Translator.Orchestrators.Finders
{
    using System;
    using System.Threading.Tasks;
    using Dynamic.Translator.Core;
    using Dynamic.Translator.Core.Dependency.Markers;
    using Dynamic.Translator.Core.Orchestrators;
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