namespace Dynamic.Translator.ViewModel
{
    #region using

    using System;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Windows;
    using Core.Dependency.Manager;
    using Core.Orchestrators;
    using Orchestrators.Observers;

    #endregion

    public partial class MainWindow
    {
        private bool isRunning;
        private ITranslatorBootstrapper translator;

        public MainWindow()
        {
            InitializeComponent();
            IocManager.Instance.Register(typeof (MainWindow), this);
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

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
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
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            translator = IocManager.Instance.Resolve<ITranslatorBootstrapper>();

            var translatorEvents = Observable
                .FromEventPattern<WhenClipboardContainsTextEventArgs>(
                    h => translator.WhenClipboardContainsTextEventHandler += h,
                    h => translator.WhenClipboardContainsTextEventHandler -= h);

            translatorEvents.Subscribe(IocManager.Instance.Resolve<Finder>());
        }
    }
}