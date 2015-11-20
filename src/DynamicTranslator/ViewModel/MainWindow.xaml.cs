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
            IocManager.Instance.Register(typeof(MainWindow), this);
            translator = IocManager.Instance.Resolve<ITranslatorBootstrapper>();
        }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            Task.Run(async () =>
            {
                await this.Dispatcher.InvokeAsync(async () =>
                  {
                      CancellationTokenSource?.Cancel(false);
                      Close();
                      if (CancellationTokenSource != null && !CancellationTokenSource.Token.CanBeCanceled)
                      {
                          translator.Dispose();
                          await IocManager.Instance.DisposeAsync();
                          GC.SuppressFinalize(this);
                          GC.Collect();
                      }
                      Application.Current.Shutdown();
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