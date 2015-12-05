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
    using Model;

    #endregion

    public partial class MainWindow
    {
        private readonly ITranslatorBootstrapper translator;
        private readonly IStartupConfiguration configuration;

        private bool isRunning;

        public MainWindow()
        {
            InitializeComponent();
            IocManager.Instance.Register(typeof(MainWindow), this);
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

        private void GithubButton_Click()
        {

        }

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                BtnSwitch.Content = "Start Translator";

                isRunning = false;

                ComboBoxLanguages.Focusable = true;
                ComboBoxLanguages.IsHitTestVisible = true;
               
                translator.Dispose();
            }
            else
            {
                BtnSwitch.Content = "Stop Translator";

                configuration.SetAndPersistConfigurationManager(nameof(configuration.ToLanguage), ((Language)ComboBoxLanguages.SelectedItem).Name);

                ComboBoxLanguages.Focusable = false;
                ComboBoxLanguages.IsHitTestVisible = false;

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

        private void GithubButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/osoykan/DynamicTranslator");
        }
        private void DonateButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=BRD64ND693C98");
        }
    }
}