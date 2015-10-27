namespace Dynamic.Translator.Orchestrators.Observers
{
    using System;
    using System.Reactive;
    using Dynamic.Translator.Core.Orchestrators;

    public class Notifier : IObserver<EventPattern<WhenNotificationAddEventArgs>>
    {
        private readonly ITranslator translator;

        public Notifier(ITranslator translator)
        {
            this.translator = translator;
        }

        public async void OnNext(EventPattern<WhenNotificationAddEventArgs> value)
        {
            await this.translator.AddNotificationAsync(value.EventArgs.Title, value.EventArgs.ImageUrl, value.EventArgs.ImageUrl);
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}