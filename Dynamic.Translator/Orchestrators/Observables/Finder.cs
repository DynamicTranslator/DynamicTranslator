namespace Dynamic.Tureng.Translator.Orchestrators.Observables
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Text;
    using System.Windows;
    using Dynamic.Translator.Core.Config;
    using Dynamic.Translator.Core.Dependency.Manager;
    using Dynamic.Translator.Core.Orchestrators;
    using Dynamic.Translator.Core.ViewModel.Constants;

    public class Finder : IObserver<EventPattern<object>>
    {
        private readonly IStartupConfiguration _configurations;
        private readonly IMeanFinderFactory meanFinderFactory;
        private readonly ITranslator translator;

        private string currentString;
        private string previousString;

        public Finder(ITranslator translator)
        {
            this.translator = translator;
            this._configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
            this.meanFinderFactory = IocManager.Instance.Resolve<IMeanFinderFactory>();
        }

        public async void OnNext(EventPattern<object> value)
        {
            if (!Clipboard.ContainsText()) return;

            this.currentString = Clipboard.GetText();
            if (this.previousString != this.currentString)
            {
                this.previousString = this.currentString;
                if (this.currentString.Length > this._configurations.SearchableCharacterLimit)
                {
                    this.translator.AddNotificationEvent(this, new WhenNotificationAddEventArgs
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
                        var mean = new StringBuilder();

                        foreach (var finder in this.meanFinderFactory.GetFinders())
                        {
                            mean.Append((await finder.Find(this.currentString)).DefaultIfEmpty(string.Empty).First().Trim());
                        }

                        if (!string.IsNullOrEmpty(mean.ToString()))
                        {
                            this.translator.AddNotificationEvent(this, new WhenNotificationAddEventArgs
                            {
                                Title = this.currentString,
                                ImageUrl = ImageUrls.NotificationUrl,
                                Message = mean.ToString()
                            });
                        }
                    }
                    else
                    {
                        this.translator.AddNotificationEvent(this, new WhenNotificationAddEventArgs
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