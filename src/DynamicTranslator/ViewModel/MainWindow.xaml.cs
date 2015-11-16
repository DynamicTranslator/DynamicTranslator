namespace DynamicTranslator.ViewModel
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
            if (CancellationTokenSource != null && !CancellationTokenSource.Token.CanBeCanceled)
            {
                translator.Dispose();
                GC.SuppressFinalize(this);
                GC.Collect();
            }
            Application.Current.Shutdown();
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
                    BtnSwitch.Content = "Stop Translator";

                    if (!translator.IsInitialized)
                        translator.Initialize();

                    isRunning = true;
                }
            },
                DispatcherPriority.Background);
        }
    }
}