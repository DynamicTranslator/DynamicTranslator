namespace Dynamic.Tureng.Translator.Orchestrator.Observable
{
    using System;
    using System.Reactive;
    using System.Windows;
    using Dynamic.Translator.Core.Config;
    using Dynamic.Translator.Core.Dependency;
    using Dynamic.Translator.Core.ViewModel;

    public class Finder : IObserver<EventPattern<object>>
    {
        private readonly IStartupConfiguration _configurations;
        private readonly ITranslator translator;

        private string currentString;
        private string previousString;

        public Finder(ITranslator translator)
        {
            this.translator = translator;
            this._configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
        }

        public void OnNext(EventPattern<object> value)
        {
            if (!Clipboard.ContainsText()) return;

            this.currentString = Clipboard.GetText();
            if (this.previousString != this.currentString)
            {
                this.previousString = this.currentString;
                if (this.currentString.Length > this._configurations.SearchableCharacterLimit)
                {
                    this.translator.WhenNotificationAddEventInvoker(this, new WhenNotificationAddEventArgs
                    {
                        Message = "You have exceed maximum character limit",
                        ImageUrl = ImageUrls.NotificationUrl,
                        Title = Titles.MaximumLimit
                    });
                }
                else
                {
                    if (!string.IsNullOrEmpty(this._configurations.ApiKey))
                    {
                        //await this.GetMeanFromTureng();
                    }
                    else
                    {
                        this.translator.WhenNotificationAddEventInvoker(this, new WhenNotificationAddEventArgs
                        {
                            Title = Titles.Warning,
                            ImageUrl = ImageUrls.NotificationUrl,
                            Message = "The Api Key cannot be NULL !"
                        });
                    }
                }
            }
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}