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
            this.InitializeComponent();
            IocManager.Instance.Register(typeof (MainWindow), this);
        }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            this.CancellationTokenSource?.Cancel(false);
            this.Close();
            Application.Current.Shutdown();
            if (this.CancellationTokenSource != null && !this.CancellationTokenSource.Token.CanBeCanceled)
            {
                GC.SuppressFinalize(this);
                GC.Collect();
            }
            base.OnClosed(e);
        }

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (this.isRunning)
            {
                this.BtnSwitch.Content = "Start Translator";
                this.isRunning = false;
                if (this.translator.IsInitialized)
                    this.translator.Dispose();
            }
            else
            {
                if (!this.translator.IsInitialized)
                    this.translator.Initialize();

                this.isRunning = true;
                this.BtnSwitch.Content = "Stop Translator";
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.translator = IocManager.Instance.Resolve<ITranslatorBootstrapper>();

            var translatorEvents = Observable
                .FromEventPattern<WhenClipboardContainsTextEventArgs>(
                    h => this.translator.WhenClipboardContainsTextEventHandler += h,
                    h => this.translator.WhenClipboardContainsTextEventHandler -= h);

            translatorEvents.Subscribe(IocManager.Instance.Resolve<Finder>());
        }
    }
}