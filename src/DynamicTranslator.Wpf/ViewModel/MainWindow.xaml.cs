using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Abp.Dependency;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Wpf.ViewModel
{
    public partial class MainWindow
    {
        private readonly IDynamicTranslatorConfiguration configurations;
        private readonly ITranslatorBootstrapper translator;
        private bool isRunning;

        public MainWindow()
        {
            InitializeComponent();
            IocManager.Instance.Register(typeof(MainWindow), this);
            translator = IocManager.Instance.Resolve<ITranslatorBootstrapper>();
            translator.SubscribeShutdownEvents();
            configurations = IocManager.Instance.Resolve<IDynamicTranslatorConfiguration>();

            foreach (var language in LanguageMapping.All.ToLanguages())
            {
                ComboBoxLanguages.Items.Add(language);
            }

            ComboBoxLanguages.SelectedValue = configurations.ApplicationConfiguration.ToLanguage.Extension;
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

                var selectedLanguageName = ((Language)ComboBoxLanguages.SelectedItem).Name;
                configurations.AppConfigManager.SaveOrUpdate("ToLanguage", selectedLanguageName);
                configurations.ApplicationConfiguration.ToLanguage = new Language(selectedLanguageName, LanguageMapping.All[selectedLanguageName]);

                PrepareTranslators();
                LockUiElements();

                Task.Run(
                    async () =>
                    {
                        await Dispatcher.InvokeAsync(
                            async () =>
                            {
                                if (!translator.IsInitialized)
                                    await translator.InitializeAsync();
                            });
                    });

                isRunning = true;
            }
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

            CheckBoxBing.IsHitTestVisible = false;
        }

        private void PrepareTranslators()
        {
            configurations.ActiveTranslatorConfiguration.PassivateAll();

            if (CheckBoxGoogleTranslate.IsChecked != null && CheckBoxGoogleTranslate.IsChecked.Value)
                configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Google);
            if (CheckBoxYandexTranslate.IsChecked != null && CheckBoxYandexTranslate.IsChecked.Value)
                configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Yandex);
            if (CheckBoxTureng.IsChecked != null && CheckBoxTureng.IsChecked.Value)
                configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Tureng);
            if (CheckBoxSesliSozluk.IsChecked != null && CheckBoxSesliSozluk.IsChecked.Value)
                configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Seslisozluk);
            if (CheckBoxBing.IsChecked != null && CheckBoxBing.IsChecked.Value)
                configurations.ActiveTranslatorConfiguration.Activate(TranslatorType.Bing);

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