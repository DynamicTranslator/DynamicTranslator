namespace Dynamic.Translator.Orchestrators.Observables
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Text;
    using System.Windows;
    using Core.Dependency.Manager;
    using Core.Extensions;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    public class Finder : IObserver<EventPattern<object>>
    {
        private readonly IMeanFinderFactory meanFinderFactory;
        private readonly ITranslator translator;
        private string currentString;


        public Finder(ITranslator translator)
        {
            if (translator == null)
                throw new ArgumentNullException(nameof(translator));

            this.translator = translator;
            this.meanFinderFactory = IocManager.Instance.Resolve<IMeanFinderFactory>();
        }

        public async void OnNext(EventPattern<object> value)
        {
            if (!Clipboard.ContainsText()) return;

            this.currentString = Clipboard.GetText().RemoveSpecialCharacters();

            var mean = new StringBuilder();

            foreach (var finder in this.meanFinderFactory.GetFinders())
            {
                var result = await finder.Find(this.currentString);
                if (result.IsSucess)
                {
                    mean.AppendLine(result.ResultMessage.DefaultIfEmpty(string.Empty).First());
                }
                else
                {
                    this.translator.AddNotification(Titles.Warning, ImageUrls.NotificationUrl, result.ResultMessage.DefaultIfEmpty(string.Empty).First());
                    break;
                }
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

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}