namespace Dynamic.Translator.Orchestrators.Observables
{
    using System;
    using System.Reactive;
    using Core.Orchestrators;

    public class Notifier : IObserver<EventPattern<WhenNotificationAddEventArgs>>
    {
        private readonly ITranslator translator;

        public Notifier(ITranslator translator)
        {
            this.translator = translator;
        }

        public async void OnNext(EventPattern<WhenNotificationAddEventArgs> value)
        {
            this.translator.AddNotification(value.EventArgs.Title, value.EventArgs.ImageUrl, value.EventArgs.ImageUrl);
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}