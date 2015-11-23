namespace DynamicTranslator.ViewModel
{
    #region using

    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using Core.Dependency.Manager;
    using Core.Orchestrators;

    #endregion

    public partial class MainWindow
    {
        private readonly ITranslatorBootstrapper translator;
        private bool isRunning;

        public MainWindow()
        {
            InitializeComponent();
            IocManager.Instance.Register(typeof (MainWindow), this);
            translator = IocManager.Instance.Resolve<ITranslatorBootstrapper>();
            translator.SubscribeShutdownEvents();
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

                Task.Run(async () =>
                {
                    await Dispatcher.InvokeAsync(async () =>
                    {
                        if (translator.IsInitialized)
                            await translator.DisposeAsync().ConfigureAwait(false);
                    });
                });
            }
            else
            {
                BtnSwitch.Content = "Stop Translator";

                Task.Run(async () =>
                {
                    await Dispatcher.InvokeAsync(async () =>
                    {
                        if (!translator.IsInitialized)
                            await translator.InitializeAsync().ConfigureAwait(false);
                    });
                });

                isRunning = true;
            }
        }
    }
}