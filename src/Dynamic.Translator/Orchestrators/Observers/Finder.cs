namespace Dynamic.Translator.Orchestrators.Observers
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Dependency.Markers;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    public class Finder : IObserver<EventPattern<WhenClipboardContainsTextEventArgs>>, ISingletonDependency
    {
        private readonly IMeanFinderFactory meanFinderFactory;
        private readonly INotifier notifier;

        public Finder(INotifier notifier, IMeanFinderFactory meanFinderFactory)
        {
            if (notifier == null)
                throw new ArgumentNullException(nameof(notifier));

            if (meanFinderFactory == null)
                throw new ArgumentNullException(nameof(meanFinderFactory));

            this.notifier = notifier;
            this.meanFinderFactory = meanFinderFactory;
        }

        public async void OnNext(EventPattern<WhenClipboardContainsTextEventArgs> value)
        {
            await Task.Run(async () =>
            {
                var currentString = value.EventArgs.CurrentString;
                var mean = new StringBuilder();

                var tasks = this.meanFinderFactory.GetFinders().Select(t => t.Find(currentString));
                var results = await Task.WhenAll(tasks);

                foreach (var result in results)
                {
                    if (result.IsSucess)
                        mean.AppendLine(result.ResultMessage.DefaultIfEmpty(string.Empty).First());
                    else
                    {
                        await this.notifier.AddNotificationAsync(Titles.Warning, ImageUrls.NotificationUrl, result.ResultMessage.DefaultIfEmpty(string.Empty).First());
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

                    await this.notifier.AddNotificationAsync(currentString, ImageUrls.NotificationUrl, mean.ToString());
                }
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