namespace DynamicTranslator.ViewModel
{
    #region using

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
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

        protected override async void OnClosed(EventArgs e)
        {
            await Task.Run(async () =>
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    CancellationTokenSource?.Cancel(false);
                    Application.Current.Shutdown();
                    Close();
                    if (CancellationTokenSource != null && !CancellationTokenSource.Token.CanBeCanceled)
                    {
                        translator.Dispose();
                        //await IocManager.Instance.DisposeAsync();
                        GC.SuppressFinalize(this);
                        GC.Collect();
                    }

                    base.OnClosed(e);
                });
            });
        }

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                BtnSwitch.Content = "Start Translator";

                isRunning = false;

                Task.Run(async () =>
                {
                    await Dispatcher.InvokeAsync(() =>
                    {
                        if (translator.IsInitialized)
                            translator.Dispose();
                    });
                });
            }
            else
            {
                BtnSwitch.Content = "Stop Translator";

                Task.Run(async () =>
                {
                    await Dispatcher.InvokeAsync(() =>
                    {
                        if (!translator.IsInitialized)
                            translator.Initialize();
                    });
                });

                isRunning = true;
            }
        }
    }
}