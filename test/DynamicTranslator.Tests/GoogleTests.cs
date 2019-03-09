using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DynamicTranslator.Configuration;
using DynamicTranslator.Google;
using DynamicTranslator.Model;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace DynamicTranslator.Tests
{
    public class GoogleTests
    {
        private static TestMessageHandler GoogleMessageHandler()
        {
            return new TestMessageHandler
            {
                Sender = message => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(
                        new Dictionary<string, object>()
                        {
                            ["sentences"] = new JArray("Sehir")
                        }))
                })
            };
        }

        [Fact]
        public async Task When_Only_Google_Enabled()
        {
            const string detectedLanguage = "en";
            const string english = "City";
            const string turkish = "sehir";
            const string result = "* sehir\r\n";

            var notifier = A.Fake<INotifier>();
            var googleAnalytics = A.Fake<IGoogleAnalyticsTracker>();
            var languageDetector = A.Fake<IGoogleLanguageDetector>();
            var resultOrganizer = A.Fake<ResultOrganizer>();
            var translator = A.Fake<ITranslator>();
            var cancellationToken = CancellationToken.None;

            A.CallTo(() => languageDetector.DetectLanguage(english, A<CancellationToken>.Ignored)).Returns(detectedLanguage);
            A.CallTo(() => translator.Translate(A<TranslateRequest>.Ignored, cancellationToken)).Returns(new TranslateResult(true, turkish));
            A.CallTo(() => translator.Type).Returns(TranslatorType.Google);

            var activeTranslatorConfiguration = new ActiveTranslatorConfiguration();
            activeTranslatorConfiguration.AddTranslator(TranslatorType.Google);
            activeTranslatorConfiguration.Activate(TranslatorType.Google);

            var wireUp = new WireUp(builder =>
            {
                builder.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("ToLanguage", "Turkish"),
                    new KeyValuePair<string, string>("FromLanguage", "English"),
                });
            }, services =>
            {
                services.RemoveAll<ITranslator>();
                services.AddTransient<IFinder, Finder>();
                services.AddTransient(sp => notifier);
                services.AddTransient(sp => googleAnalytics);
                services.AddTransient(sp => languageDetector);
                services.AddTransient(sp => activeTranslatorConfiguration);
                services.AddTransient(sp => resultOrganizer);
                services.AddSingleton(sp => translator);
            })
            {
                MessageHandler = GoogleMessageHandler()
            };

            var sut = wireUp.ServiceProvider.GetService<IFinder>();
            await sut.Find(english, cancellationToken);
            A.CallTo(() => notifier.AddNotification(english, ImageUrls.NotificationUrl, result)).MustHaveHappenedOnceExactly();
            A.CallTo(() => translator.Translate(A<TranslateRequest>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}