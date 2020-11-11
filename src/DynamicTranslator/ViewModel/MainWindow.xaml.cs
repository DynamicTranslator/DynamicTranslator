namespace DynamicTranslator.ViewModel
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Core;
    using Core.Configuration;
    using Core.Model;
    using Extensions;
    using Octokit;
    using Language = Core.Model.Language;

    public partial class MainWindow
    {
        readonly ActiveTranslatorConfiguration activeTranslatorConfiguration;
        readonly IApplicationConfiguration applicationConfiguration;
        readonly TranslatorBootstrapper translator;
        GitHubClient gitHubClient;
        Func<string, bool> isNewVersion;
        bool isRunning;

        public MainWindow(ActiveTranslatorConfiguration activeTranslatorConfiguration,
            IApplicationConfiguration applicationConfiguration,
            TranslatorBootstrapper translator)
        {
            this.activeTranslatorConfiguration = activeTranslatorConfiguration;
            this.applicationConfiguration = applicationConfiguration;
            this.translator = translator;
        }

        protected override void OnInitialized(EventArgs e)
        {
            InitializeComponent();
            base.OnInitialized(e);

            this.gitHubClient = new GitHubClient(new ProductHeaderValue("DynamicTranslator"));
            this.isNewVersion = version =>
            {
                var currentVersion = new Version(ApplicationVersion.GetCurrentVersion());
                var newVersion = new Version(version);

                return newVersion > currentVersion;
            };

            FillLanguageCombobox();
            InitializeVersionChecker();
        }

        void BtnSwitchClick(object sender, RoutedEventArgs e)
        {
            if (this.isRunning)
            {
                this.BtnSwitch.Content = "Start Translator";

                this.isRunning = false;

                UnlockUiElements();

                this.translator.Stop();
            }
            else
            {
                this.BtnSwitch.Content = "Stop Translator";

                string selectedLanguageName = ((Language) this.ComboBoxLanguages.SelectedItem).Name;
                this.applicationConfiguration.ToLanguage =
                    new Language(selectedLanguageName, LanguageMapping.All[selectedLanguageName]);

                PrepareTranslators();
                LockUiElements();

                if (!this.translator.IsInitialized)
                    this.translator.Initialize();

                this.isRunning = true;
            }
        }

        void FillLanguageCombobox()
        {
            foreach (Language language in LanguageMapping.All.ToLanguages())
                this.ComboBoxLanguages.Items.Add(language);

            this.ComboBoxLanguages.SelectedValue = this.applicationConfiguration.ToLanguage.Extension;
        }

        Task<Release> GetRelease()
        {
            return this.gitHubClient.Repository.Release.GetLatest("DynamicTranslator", "DynamicTranslator");
        }

        void InitializeVersionChecker()
        {
            this.BtnNewVersion.Visibility = Visibility.Hidden;
            CheckVersion();
        }

        void CheckVersion()
        {
            Release release = GetRelease().Result;

            string incomingVersion = release.TagName;

            if (this.isNewVersion(incomingVersion))
            {
                this.DispatchingAsync(() =>
                {
                    this.BtnNewVersion.Visibility = Visibility.Visible;
                    this.BtnNewVersion.Content = $"Update to {incomingVersion}.";
                    this.applicationConfiguration.UpdateLink = release.Assets.FirstOrDefault()?.BrowserDownloadUrl;
                });
            }
        }

        void LockUiElements()
        {
            this.DispatchingAsync(() =>
            {
                this.ComboBoxLanguages.Focusable = false;
                this.ComboBoxLanguages.IsHitTestVisible = false;
                this.CheckBoxGoogleTranslate.IsHitTestVisible = false;
                this.CheckBoxTureng.IsHitTestVisible = false;
                this.CheckBoxSesliSozluk.IsHitTestVisible = false;
                this.CheckBoxPrompt.IsHitTestVisible = false;
            });
        }

        void PrepareTranslators()
        {
            this.activeTranslatorConfiguration.DeActivate();

            if (this.CheckBoxGoogleTranslate.IsChecked != null && this.CheckBoxGoogleTranslate.IsChecked.Value)
                this.activeTranslatorConfiguration.AddTranslator(TranslatorType.Google);

            if (this.CheckBoxTureng.IsChecked != null && this.CheckBoxTureng.IsChecked.Value)
                this.activeTranslatorConfiguration.AddTranslator(TranslatorType.Tureng);

            if (this.CheckBoxSesliSozluk.IsChecked != null && this.CheckBoxSesliSozluk.IsChecked.Value)
                this.activeTranslatorConfiguration.AddTranslator(TranslatorType.SesliSozluk);

            if (this.CheckBoxPrompt.IsChecked != null && this.CheckBoxPrompt.IsChecked.Value)
                this.activeTranslatorConfiguration.AddTranslator(TranslatorType.Prompt);

            if (!this.activeTranslatorConfiguration.ActiveTranslators.Any())
            {
                this.activeTranslatorConfiguration.AddTranslator(TranslatorType.Google);
                this.activeTranslatorConfiguration.AddTranslator(TranslatorType.Yandex);
                this.activeTranslatorConfiguration.AddTranslator(TranslatorType.Tureng);
                this.activeTranslatorConfiguration.AddTranslator(TranslatorType.SesliSozluk);
                this.activeTranslatorConfiguration.AddTranslator(TranslatorType.Prompt);
            }
        }

        void UnlockUiElements()
        {
            this.ComboBoxLanguages.Focusable = true;
            this.ComboBoxLanguages.IsHitTestVisible = true;
            this.CheckBoxGoogleTranslate.IsHitTestVisible = true;
            this.CheckBoxTureng.IsHitTestVisible = true;
            this.CheckBoxSesliSozluk.IsHitTestVisible = true;
            this.CheckBoxPrompt.IsHitTestVisible = true;
        }

        void BtnNewVersion_Click(object sender, RoutedEventArgs e)
        {
            string updateLink = this.applicationConfiguration.UpdateLink;
            if (!string.IsNullOrEmpty(updateLink)) Process.Start(updateLink);
        }
    }
}