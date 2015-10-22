namespace Dynamic.Tureng.Translator.Orchestrator.Observables
{
    using System;
    using System.Reactive;
    using Dynamic.Translator.Core.ViewModel.Interfaces;
    using Notification = Dynamic.Translator.Core.ViewModel.Notification;

    public class Notifier : IObserver<EventPattern<WhenNotificationAddEventArgs>>
    {
        private readonly IGrowlNotifications growlNotifications;
        private ITranslator translator;

        public Notifier(ITranslator translator, IGrowlNotifications growlNotifications)
        {
            this.translator = translator;
            this.growlNotifications = growlNotifications;
        }

        public void OnNext(EventPattern<WhenNotificationAddEventArgs> value)
        {
            this.growlNotifications.AddNotificationSync(new Notification
            {
                Title = value.EventArgs.Title,
                ImageUrl = value.EventArgs.ImageUrl,
                Message = value.EventArgs.Message
            });
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}