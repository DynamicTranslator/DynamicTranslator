using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        private readonly ICacheManager _cacheManager;
        private readonly IDynamicTranslatorConfiguration _configurations;
        private readonly ITranslatorBootstrapper _translator;
        private bool _isRunning;

        public MainWindow()
        {
            InitializeComponent();

            IocManager.Instance.Register(typeof(MainWindow), this);
            _configurations = IocManager.Instance.Resolve<IDynamicTranslatorConfiguration>();
            _translator = IocManager.Instance.Resolve<ITranslatorBootstrapper>();
            _translator.SubscribeShutdownEvents();
            _cacheManager = IocManager.Instance.Resolve<ICacheManager>();

            FillLanguageCombobox();

            this.DispatchingAsync(async () => await InitializeVersionChecker());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
            base.OnClosing(e);
        }

        private void BtnSwitchClick(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
            {
                _btnSwitch.Content = "Start Translator";

                _isRunning = false;

                UnlockUiElements();

                _translator.Dispose();
            }
            else
            {
                _btnSwitch.Content = "Stop Translator";

                string selectedLanguageName = ((Language)_comboBoxLanguages.SelectedItem).Name;
                _configurations.AppConfigManager.SaveOrUpdate("ToLanguage", selectedLanguageName);
                _configurations.ApplicationConfiguration.ToLanguage = new Language(selectedLanguageName, LanguageMapping.All[selectedLanguageName]);

                PrepareTranslators();
                LockUiElements();

                this.DispatchingAsync(async () =>
                {
                    if (!_translator.IsInitialized)
                    {
                        await _translator.InitializeAsync();
                    }
                });

                _isRunning = true;
            }
        }

        private void FillLanguageCombobox()
        {
            foreach (Language language in LanguageMapping.All.ToLanguages())
            {
                _comboBoxLanguages.Items.Add(language);
            }

            _comboBoxLanguages.SelectedValue = _configurations.ApplicationConfiguration.ToLanguage.Extension;
        }

        private Task<Release> GetReleaseFromCache(GitHubClient gitHubClient)
        {
            return _cacheManager.GetCache(CacheNames.ReleaseCache)
                                .GetAsync(CacheNames.ReleaseCache,
                                    async () =>
                                        await gitHubClient
                                            .Repository
                                            .Release
                                            .GetLatest(
                                                _configurations.ApplicationConfiguration.GitHubRepositoryOwnerName,
                                                _configurations.ApplicationConfiguration.GitHubRepositoryName));
        }

        private void GithubButtonClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/DynamicTranslator/DynamicTranslator");
        }

        private async Task InitializeVersionChecker()
        {
            _newVersionButton.Visibility = Visibility.Hidden;
            await CheckVersion();
        }

        private async Task CheckVersion()
        {
            using (IScopedIocResolver scope = IocManager.Instance.CreateScope())
            {
                Release release = await GetReleaseFromCache(scope.Resolve<GitHubClient>());

                string incomingVersion = release.TagName;

                if (scope.Resolve<IsNewVersion>()(incomingVersion))
                {
                    await this.DispatchingAsync(() =>
                    {
                        _newVersionButton.Visibility = Visibility.Visible;
                        _newVersionButton.Content = $"A new version {incomingVersion} released, update now!";
                        _configurations.ApplicationConfiguration.UpdateLink = release.Assets.FirstOrDefault()?.BrowserDownloadUrl;
                    });
                }
            }
        }

        private void LockUiElements()
        {
            _comboBoxLanguages.Focusable = false;
            _comboBoxLanguages.IsHitTestVisible = false;
            _checkBoxGoogleTranslate.IsHitTestVisible = false;
            _checkBoxTureng.IsHitTestVisible = false;
            _checkBoxYandexTranslate.IsHitTestVisible = false;
            _checkBoxSesliSozluk.IsHitTestVisible = false;
            _checkBoxPrompt.IsHitTestVisible = false;
        }

        private void NewVersionButtonClick(object sender, RoutedEventArgs e)
        {
            string updateLink = _configurations.ApplicationConfiguration.UpdateLink;
            if (!string.IsNullOrEmpty(updateLink))
            {
                Process.Start(updateLink);
            }
        }

        private void PrepareTranslators()
        {
            _configurations.ActiveTranslatorConfiguration.PassivateAll();

            if (_checkBoxGoogleTranslate.IsChecked != null && _checkBoxGoogleTranslate.IsChecked.Value)
            {
                _configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Google);
            }

            if (_checkBoxYandexTranslate.IsChecked != null && _checkBoxYandexTranslate.IsChecked.Value)
            {
                _configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Yandex);
            }

            if (_checkBoxTureng.IsChecked != null && _checkBoxTureng.IsChecked.Value)
            {
                _configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Tureng);
            }

            if (_checkBoxSesliSozluk.IsChecked != null && _checkBoxSesliSozluk.IsChecked.Value)
            {
                _configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.SesliSozluk);
            }

            if (_checkBoxPrompt.IsChecked != null && _checkBoxPrompt.IsChecked.Value)
            {
                _configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Prompt);
            }

            if (!_configurations.ActiveTranslatorConfiguration.ActiveTranslators.Any())
            {
                foreach (object value in Enum.GetValues(typeof(TranslatorType)))
                {
                    _configurations.ActiveTranslatorConfiguration.Activate((TranslatorType)value);
                }
            }
        }

        private void UnlockUiElements()
        {
            _comboBoxLanguages.Focusable = true;
            _comboBoxLanguages.IsHitTestVisible = true;
            _checkBoxGoogleTranslate.IsHitTestVisible = true;
            _checkBoxTureng.IsHitTestVisible = true;
            _checkBoxYandexTranslate.IsHitTestVisible = true;
            _checkBoxSesliSozluk.IsHitTestVisible = true;
            _checkBoxPrompt.IsHitTestVisible = true;
        }
    }
}
