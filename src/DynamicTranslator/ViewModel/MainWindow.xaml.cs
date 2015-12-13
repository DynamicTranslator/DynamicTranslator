namespace DynamicTranslator.ViewModel
{
    #region using

    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using Core.Config;
    using Core.Dependency.Manager;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;
    using Model;

    #endregion

    public partial class MainWindow
    {
        private readonly IStartupConfiguration configuration;
        private readonly ITranslatorBootstrapper translator;
        private bool isRunning;

        public MainWindow()
        {
            InitializeComponent();
            IocManager.Instance.Register(typeof (MainWindow), this);
            translator = IocManager.Instance.Resolve<ITranslatorBootstrapper>();
            translator.SubscribeShutdownEvents();
            configuration = IocManager.Instance.Resolve<IStartupConfiguration>();
            foreach (var language in configuration.LanguageMap)
            {
                ComboBoxLanguages.Items.Add(new Language(language.Key, language.Value));
            }

            ComboBoxLanguages.SelectedValue = configuration.ToLanguageExtension;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
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

                configuration.SetAndPersistConfigurationManager(nameof(configuration.ToLanguage), ((Language) ComboBoxLanguages.SelectedItem).Name);

                PrepairTranslators();
                LockUiElements();

                Task.Run(async () =>
                {
                    await Dispatcher.InvokeAsync(async () =>
                    {
                        if (!translator.IsInitialized)
                            await translator.InitializeAsync();
                    });
                });

                isRunning = true;
            }
        }

        private void DonateButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=6U2T5SPVDZ7TW");
        }

        private void GithubButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/osoykan/DynamicTranslator");
        }

        private void LockUiElements()
        {
            ComboBoxLanguages.Focusable = false;
            ComboBoxLanguages.IsHitTestVisible = false;
            CheckBoxGoogleTranslate.IsHitTestVisible = false;
            CheckBoxTureng.IsHitTestVisible = false;
            CheckBoxYandexTranslate.IsHitTestVisible = false;
            CheckBoxSesliSozluk.IsHitTestVisible = false;
        }

        private void PrepairTranslators()
        {
            configuration.ClearActiveTranslators();

            if (CheckBoxGoogleTranslate.IsChecked != null && CheckBoxGoogleTranslate.IsChecked.Value)
            {
                configuration.AddTranslator(TranslatorType.GOOGLE);
            }
            if (CheckBoxYandexTranslate.IsChecked != null && CheckBoxYandexTranslate.IsChecked.Value)
            {
                configuration.AddTranslator(TranslatorType.YANDEX);
            }
            if (CheckBoxTureng.IsChecked != null && CheckBoxTureng.IsChecked.Value)
            {
                configuration.AddTranslator(TranslatorType.TURENG);
            }
            if (CheckBoxSesliSozluk.IsChecked != null && CheckBoxSesliSozluk.IsChecked.Value)
            {
                configuration.AddTranslator(TranslatorType.SESLISOZLUK);
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
        }
    }
}