namespace Dynamic.Translator.Orchestrators.Observables
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Dependency.Manager;
    using Core.Extensions;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    public class Finder : IObserver<EventPattern<WhenClipboardContainsTextEventArgs>>
    {
        private readonly IMeanFinderFactory meanFinderFactory;
        private readonly ITranslator translator;
        private string currentString;
        private string previousString;

        public Finder(ITranslator translator)
        {
            if (translator == null)
                throw new ArgumentNullException(nameof(translator));

            this.translator = translator;
            this.meanFinderFactory = IocManager.Instance.Resolve<IMeanFinderFactory>();
        }

        public async void OnNext(EventPattern<WhenClipboardContainsTextEventArgs> value)
        {
            this.currentString = value.EventArgs.CurrentString.RemoveSpecialCharacters();

            if (this.currentString != this.previousString)
            {
                this.previousString = this.currentString;

                var mean = new StringBuilder();

                var tasks = this.meanFinderFactory.GetFinders().Select(t => t.Find(this.currentString));
                var results = await Task.WhenAll(tasks);

                foreach (var result in results)
                {
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
                        .Where(s => s != string.Empty && s != this.currentString.Trim() && s != "Translation")
                        .Distinct()
                        .ToList();

                    mean.Clear();
                    means.ForEach(m => mean.AppendLine("* " + m.ToLower()));

                    this.translator.AddNotification(this.currentString, ImageUrls.NotificationUrl, mean.ToString());
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