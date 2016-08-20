using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Abp.Dependency;
using Abp.Runtime.Caching;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;
using DynamicTranslator.LanguageManagement;
using DynamicTranslator.Wpf.Extensions;

using Octokit;

using Language = DynamicTranslator.LanguageManagement.Language;

namespace DynamicTranslator.Wpf.ViewModel
{
    public partial class MainWindow
    {
        private readonly ICacheManager cacheManager;
        private readonly IDynamicTranslatorConfiguration configurations;
        private readonly ITranslatorBootstrapper translator;
        private bool isRunning;
        private IDisposable versionObserver;

        public MainWindow()
        {
            InitializeComponent();

            IocManager.Instance.Register(typeof(MainWindow), this);
            configurations = IocManager.Instance.Resolve<IDynamicTranslatorConfiguration>();
            translator = IocManager.Instance.Resolve<ITranslatorBootstrapper>();
            translator.SubscribeShutdownEvents();
            cacheManager = IocManager.Instance.Resolve<ICacheManager>();

            FillLanguageCombobox();
            InitializeVersionChecker();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            versionObserver.Dispose();
            Dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
            base.OnClosing(e);
        }

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                BtnSwitch.Content = "Start Translator";

                isRunning = false;

                UnlockUiElements();

                translator.Dispose();
            }
            else
            {
                BtnSwitch.Content = "Stop Translator";

                var selectedLanguageName = ((Language)ComboBoxLanguages.SelectedItem).Name;
                configurations.AppConfigManager.SaveOrUpdate("ToLanguage", selectedLanguageName);
                configurations.ApplicationConfiguration.ToLanguage = new Language(selectedLanguageName, LanguageMapping.All[selectedLanguageName]);

                PrepareTranslators();
                LockUiElements();

                this.DispatchingAsync(async () =>
                {
                    if (!translator.IsInitialized)
                    {
                        await translator.InitializeAsync();
                    }
                });

                isRunning = true;
            }
        }

        private void FillLanguageCombobox()
        {
            foreach (var language in LanguageMapping.All.ToLanguages())
            {
                ComboBoxLanguages.Items.Add(language);
            }

            ComboBoxLanguages.SelectedValue = configurations.ApplicationConfiguration.ToLanguage.Extension;
        }

        private Task<Release> GetReleaseFromCache(IDisposableDependencyObjectWrapper<GitHubClient> gitHubClient)
        {
            return cacheManager.GetCache(CacheNames.ReleaseCache)
                                .GetAsync(CacheNames.ReleaseCache,
                                    async () => await gitHubClient.Object
                                        .Repository
                                        .Release
                                        .GetLatest(
                                            configurations.ApplicationConfiguration.GitHubRepositoryOwnerName,
                                            configurations.ApplicationConfiguration.GitHubRepositoryName));
        }

        private void GithubButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/osoykan/DynamicTranslator");
        }

        private void InitializeVersionChecker()
        {
            NewVersionButton.Visibility = Visibility.Hidden;

            versionObserver = Observable.Interval(TimeSpan.FromMinutes(1))
                                         .Subscribe(async x =>
                                         {
                                             using (var gitHubClient = IocManager.Instance.ResolveAsDisposable<GitHubClient>())
                                             {
                                                 var release = await GetReleaseFromCache(gitHubClient);

                                                 var version = release.TagName;

                                                 if (!ApplicationVersion.Is(version))
                                                 {
                                                     await this.DispatchingAsync(() =>
                                                     {
                                                         NewVersionButton.Visibility = Visibility.Visible;
                                                         NewVersionButton.Content = $"A new version {version} released, update now!";
                                                         configurations.ApplicationConfiguration.UpdateLink = release.Assets.FirstOrDefault()?.BrowserDownloadUrl;
                                                     });
                                                 }
                                             }
                                         });
        }

        private void LockUiElements()
        {
            ComboBoxLanguages.Focusable = false;
            ComboBoxLanguages.IsHitTestVisible = false;
            CheckBoxGoogleTranslate.IsHitTestVisible = false;
            CheckBoxTureng.IsHitTestVisible = false;
            CheckBoxYandexTranslate.IsHitTestVisible = false;
            CheckBoxSesliSozluk.IsHitTestVisible = false;
            CheckBoxBing.IsHitTestVisible = false;
        }

        private void NewVersionButton_Click(object sender, RoutedEventArgs e)
        {
            var updateLink = configurations.ApplicationConfiguration.UpdateLink;
            if (!string.IsNullOrEmpty(updateLink))
            {
                Process.Start(updateLink);
            }
        }

        private void PrepareTranslators()
        {
            configurations.ActiveTranslatorConfiguration.PassivateAll();

            if ((CheckBoxGoogleTranslate.IsChecked != null) && CheckBoxGoogleTranslate.IsChecked.Value)
            {
                configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Google);
            }
            if ((CheckBoxYandexTranslate.IsChecked != null) && CheckBoxYandexTranslate.IsChecked.Value)
            {
                configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Yandex);
            }
            if ((CheckBoxTureng.IsChecked != null) && CheckBoxTureng.IsChecked.Value)
            {
                configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Tureng);
            }
            if ((CheckBoxSesliSozluk.IsChecked != null) && CheckBoxSesliSozluk.IsChecked.Value)
            {
                configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.SesliSozluk);
            }
            if ((CheckBoxBing.IsChecked != null) && CheckBoxBing.IsChecked.Value)
            {
                configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Bing);
            }

            if (!configurations.ActiveTranslatorConfiguration.ActiveTranslators.Any())
            {
                foreach (var value in Enum.GetValues(typeof(TranslatorType)))
                {
                    configurations.ActiveTranslatorConfiguration.Activate((TranslatorType)value);
                }
            }
        }

        private void UnlockUiElements()
        {
            ComboBoxLanguages.Focusable = true;
            ComboBoxLanguages.IsHitTestVisible = true;
            CheckBoxGoogleTranslate.IsHitTestVisible = true;
            CheckBoxTureng.IsHitTestVisible = true;
            CheckBoxYandexTranslate.IsHitTestVisible = true;
            CheckBoxSesliSozluk.IsHitTestVisible = true;
            CheckBoxBing.IsHitTestVisible = true;
        }
    }
}