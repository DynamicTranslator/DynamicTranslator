using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DynamicTranslator.Configuration;
using DynamicTranslator.Model;
using DynamicTranslator.Wpf.Extensions;
using Octokit;
using Language = DynamicTranslator.Model.Language;

namespace DynamicTranslator.Wpf.ViewModel
{
    public partial class MainWindow : Window
    {
        private readonly ActiveTranslatorConfiguration _activeTranslatorConfiguration;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private GitHubClient _gitHubClient;
        private Func<string, bool> _isNewVersion;
        private bool _isRunning;
        private readonly TranslatorBootstrapper _translator;

        public MainWindow(ActiveTranslatorConfiguration activeTranslatorConfiguration, ApplicationConfiguration applicationConfiguration, TranslatorBootstrapper translator)
        {
            _activeTranslatorConfiguration = activeTranslatorConfiguration;
            _applicationConfiguration = applicationConfiguration;
            _translator = translator;
        }

        protected override void OnInitialized(EventArgs e)
        {
            InitializeComponent();
            base.OnInitialized(e);
            
            _gitHubClient = new GitHubClient(new ProductHeaderValue("DynamicTranslator"));
            _isNewVersion = version =>
            {
                var currentVersion = new Version(ApplicationVersion.GetCurrentVersion());
                var newVersion = new Version(version);

                return newVersion > currentVersion;
            };

            FillLanguageCombobox();
            //InitializeVersionChecker();
        }

        private void BtnSwitchClick(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
            {
                BtnSwitch.Content = "Start Translator";

                _isRunning = false;

                UnlockUiElements();
            }
            else
            {
                BtnSwitch.Content = "Stop Translator";

                var selectedLanguageName = ((Language) ComboBoxLanguages.SelectedItem).Name;
                _applicationConfiguration.ToLanguage =
                    new Language(selectedLanguageName, LanguageMapping.All[selectedLanguageName]);

                PrepareTranslators();
                LockUiElements();

                this.DispatchingAsync(() =>
                {
                    if (!_translator.IsInitialized)
                    {
                        _translator.Initialize();
                    }
                });

                _isRunning = true;
            }
        }

        private void FillLanguageCombobox()
        {
            foreach (var language in LanguageMapping.All.ToLanguages()) ComboBoxLanguages.Items.Add(language);

            ComboBoxLanguages.SelectedValue = _applicationConfiguration.ToLanguage.Extension;
        }

        private Task<Release> GetRelease()
        {
            return _gitHubClient.Repository.Release.GetLatest("DynamicTranslator", "DynamicTranslator");
        }

        private void InitializeVersionChecker()
        {
            //NewVersionButton.Visibility = Visibility.Hidden;
            CheckVersion();
        }

        private void CheckVersion()
        {
            var release = GetRelease().Result;

            var incomingVersion = release.TagName;

            if (_isNewVersion(incomingVersion))
                this.DispatchingAsync(() =>
                {
                    //NewVersionButton.Visibility = Visibility.Visible;
                    //NewVersionButton.Content = $"A new version {incomingVersion} released, update now!";
                    _applicationConfiguration.UpdateLink = release.Assets.FirstOrDefault()?.BrowserDownloadUrl;
                });
        }

        private void LockUiElements()
        {
            this.DispatchingAsync(() =>
            {
                ComboBoxLanguages.Focusable = false;
                ComboBoxLanguages.IsHitTestVisible = false;
                CheckBoxGoogleTranslate.IsHitTestVisible = false;
                CheckBoxTureng.IsHitTestVisible = false;
                CheckBoxYandexTranslate.IsHitTestVisible = false;
                CheckBoxSesliSozluk.IsHitTestVisible = false;
                CheckBoxPrompt.IsHitTestVisible = false;
            });
        }

        private void NewVersionButtonClick(object sender, RoutedEventArgs e)
        {
            var updateLink = _applicationConfiguration.UpdateLink;
            if (!string.IsNullOrEmpty(updateLink)) Process.Start(updateLink);
        }

        private void PrepareTranslators()
        {
            _activeTranslatorConfiguration.DeActivate();

            if (CheckBoxGoogleTranslate.IsChecked != null && CheckBoxGoogleTranslate.IsChecked.Value)
            {
                _activeTranslatorConfiguration.AddTranslator(TranslatorType.Google);
            }

            if (CheckBoxYandexTranslate.IsChecked != null && CheckBoxYandexTranslate.IsChecked.Value)
            {
                _activeTranslatorConfiguration.AddTranslator(TranslatorType.Yandex);
            }

            if (CheckBoxTureng.IsChecked != null && CheckBoxTureng.IsChecked.Value)
            {
                _activeTranslatorConfiguration.AddTranslator(TranslatorType.Tureng);
            }

            if (CheckBoxSesliSozluk.IsChecked != null && CheckBoxSesliSozluk.IsChecked.Value)
            {
                _activeTranslatorConfiguration.AddTranslator(TranslatorType.SesliSozluk);
            }

            if (CheckBoxPrompt.IsChecked != null && CheckBoxPrompt.IsChecked.Value)
            {
                _activeTranslatorConfiguration.AddTranslator(TranslatorType.Prompt);
            }

            if (!_activeTranslatorConfiguration.ActiveTranslators.Any())
            {
                _activeTranslatorConfiguration.AddTranslator(TranslatorType.Google);
                _activeTranslatorConfiguration.AddTranslator(TranslatorType.Yandex);
                _activeTranslatorConfiguration.AddTranslator(TranslatorType.Tureng);
                _activeTranslatorConfiguration.AddTranslator(TranslatorType.SesliSozluk);
                _activeTranslatorConfiguration.AddTranslator(TranslatorType.Prompt);
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
            CheckBoxPrompt.IsHitTestVisible = true;
        }
    }
}