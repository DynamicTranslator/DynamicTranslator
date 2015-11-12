namespace Dynamic.Translator.ViewModel
{
    #region using

    using System;
    using System.Threading;
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
        }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            CancellationTokenSource?.Cancel(false);
            Close();
            Application.Current.Shutdown();
            if (CancellationTokenSource != null && !CancellationTokenSource.Token.CanBeCanceled)
            {
                GC.SuppressFinalize(this);
                GC.Collect();
            }
            base.OnClosed(e);
        }

        private async void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                if (isRunning)
                {
                    BtnSwitch.Content = "Start Translator";
                    isRunning = false;
                    if (translator.IsInitialized)
                        translator.Dispose();
                }
                else
                {
                    if (!translator.IsInitialized)
                        translator.Initialize();

                    isRunning = true;
                    BtnSwitch.Content = "Stop Translator";
                }
            },
                DispatcherPriority.Background);
        }
    }
}