namespace Dynamic.Translator.Orchestrators.Observables
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Text;
    using System.Windows;
    using Core.Config;
    using Core.Dependency.Manager;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

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
                    this.translator.AddNotification(Titles.MaximumLimit, ImageUrls.NotificationUrl, "You have exceed maximum character limit");
                }
                else
                {
                    if (!string.IsNullOrEmpty(this._configurations.ApiKey))
                    {
                        var mean = new StringBuilder();

                        foreach (var finder in this.meanFinderFactory.GetFinders())
                        {
                            mean.AppendLine((await finder.Find(this.currentString)).DefaultIfEmpty(string.Empty).First().Trim());
                        }

                        if (!string.IsNullOrEmpty(mean.ToString()))
                        {
                            var means = mean.ToString().Split('\r')
                                 .Select(x => x.Trim())
                                 .Where(s => s != string.Empty && s != this.currentString.Trim())
                                 .Distinct()
                                 .ToList();

                            mean.Clear();
                            means.ForEach(m => mean.AppendLine(m));

                            this.translator.AddNotification(this.currentString, ImageUrls.NotificationUrl, mean.ToString());
                        }
                    }
                    else
                    {
                        this.translator.AddNotification(Titles.Warning, ImageUrls.NotificationUrl, "The Api Key cannot be NULL !");
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