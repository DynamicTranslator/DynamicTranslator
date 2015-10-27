namespace Dynamic.Translator.Orchestrators.Observers
{
    #region

    using System;
    using System.Linq;
    using System.Reactive;
    using System.Text;
    using System.Threading.Tasks;
    using Dynamic.Translator.Core.Dependency.Manager;
    using Dynamic.Translator.Core.Extensions;
    using Dynamic.Translator.Core.Orchestrators;
    using Dynamic.Translator.Core.ViewModel.Constants;

    #endregion

    public class Finder : IObserver<EventPattern<WhenClipboardContainsTextEventArgs>>
    {
        private readonly IMeanFinderFactory meanFinderFactory;
        private readonly ITranslator translator;
        private string previousString;

        public Finder(ITranslator translator)
        {
            if (translator == null)
            {
                throw new ArgumentNullException(nameof(translator));
            }

            this.translator = translator;
            this.meanFinderFactory = IocManager.Instance.Resolve<IMeanFinderFactory>();
        }

        public async void OnNext(EventPattern<WhenClipboardContainsTextEventArgs> value)
        {
            var currentString = value.EventArgs.CurrentString.RemoveSpecialCharacters();

            if (currentString != this.previousString)
            {
                this.previousString = currentString;

                var mean = new StringBuilder();

                var tasks = this.meanFinderFactory.GetFinders().Select(t => t.Find(currentString));
                var results = await Task.WhenAll(tasks);

                foreach (var result in results)
                {
                    if (result.IsSucess)
                    {
                        mean.AppendLine(result.ResultMessage.DefaultIfEmpty(string.Empty).First());
                    }
                    else
                    {
                        await this.translator.AddNotificationAsync(Titles.Warning, ImageUrls.NotificationUrl,
                            result.ResultMessage.DefaultIfEmpty(string.Empty).First());
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(mean.ToString()))
                {
                    var means = mean.ToString().Split('\r')
                        .Select(x => x.Trim())
                        .Where(s => s != string.Empty && s != currentString.Trim() && s != "Translation")
                        .Distinct()
                        .ToList();

                    mean.Clear();
                    means.ForEach(m => mean.AppendLine("* " + m.ToLower()));

                    await this.translator.AddNotificationAsync(currentString, ImageUrls.NotificationUrl, mean.ToString());
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
