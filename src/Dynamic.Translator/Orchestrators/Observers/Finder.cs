using System;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Dynamic.Translator.Core.Dependency.Markers;
using Dynamic.Translator.Core.Orchestrators;
using Dynamic.Translator.Core.ViewModel.Constants;

namespace Dynamic.Translator.Orchestrators.Observers
{
    public class Finder : IObserver<EventPattern<WhenClipboardContainsTextEventArgs>>, ISingletonDependency
    {
        private readonly IMeanFinderFactory meanFinderFactory;
        private readonly INotifier notifier;
        private string previousString;

        public Finder(INotifier notifier, IMeanFinderFactory meanFinderFactory)
        {
            if (notifier == null)
                throw new ArgumentNullException(nameof(notifier));

            if (meanFinderFactory == null)
                throw new ArgumentNullException(nameof(meanFinderFactory));

            this.notifier = notifier;
            this.meanFinderFactory = meanFinderFactory;
        }

        public void OnNext(EventPattern<WhenClipboardContainsTextEventArgs> value)
        {
            Task.Run(async () =>
            {
                var currentString = value.EventArgs.CurrentString;

                if (previousString == currentString)
                    return;

                previousString = currentString;

                var mean = new StringBuilder();

                var tasks = meanFinderFactory.GetFinders().Select(t => t.Find(currentString));
                var results = await Task.WhenAll(tasks);

                foreach (var result in results)
                {
                    if (result.IsSucess)
                        mean.AppendLine(result.ResultMessage.DefaultIfEmpty(string.Empty).First());
                    else
                    {
                        await
                            notifier.AddNotificationAsync(Titles.Warning, ImageUrls.NotificationUrl,
                                result.ResultMessage.DefaultIfEmpty(string.Empty).First());
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(mean.ToString()))
                {
                    var means = mean.ToString().Split('\r')
                        .Select(x => x.Trim().ToLower())
                        .Where(s => s != string.Empty && s != currentString.Trim() && s != "Translation")
                        .Distinct()
                        .ToList();

                    mean.Clear();
                    means.ForEach(m => mean.AppendLine("* " + m.ToLower()));

                    await notifier.AddNotificationAsync(currentString, ImageUrls.NotificationUrl, mean.ToString());
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