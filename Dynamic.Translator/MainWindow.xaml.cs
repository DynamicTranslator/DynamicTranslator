namespace Dynamic.Tureng.Translator
{
    #region using

    using System;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Forms;
    using Dynamic.Translator.Core.Config;
    using Dynamic.Translator.Core.Dependency.Manager;
    using Dynamic.Translator.Core.Orchestrators;
    using Orchestrators;
    using Orchestrators.Observables;
    using Application = System.Windows.Application;

    #endregion

    public partial class MainWindow
    {
        private CancellationToken cancellationToken;
        private CancellationTokenSource cancellationTokenSource;
        private bool isRunning;
        private ITranslator translator;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (this.cancellationToken.CanBeCanceled)
            {
                this.cancellationTokenSource.Cancel();
            }
            this.Close();
            GC.Collect();
            GC.SuppressFinalize(this);
            Application.Current.Shutdown();
        }

        private void InitializeCopyPasteListenerAsync()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.cancellationToken = this.cancellationTokenSource.Token;

            Task.Run(() =>
            {
                while (true)
                {
                    if (this.cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    Thread.Sleep(740);
                    SendKeys.SendWait("^c");
                }
            }, this.cancellationToken);
        }

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (this.isRunning)
            {
                this.BtnSwitch.Content = "Start Translator";
                if (this.cancellationToken.CanBeCanceled)
                {
                    this.cancellationTokenSource.Cancel();
                }
                this.isRunning = false;
                if (this.translator.IsInitialized)
                {
                    this.translator.Dispose();
                }
                this.RichCurrentText.Document.Blocks.Clear();
            }
            else
            {
                if (!this.translator.IsInitialized)
                {
                    this.translator.Initialize();
                }

                this.isRunning = true;
                this.BtnSwitch.Content = "Stop Translator";
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.translator = new Translator(this,
                IocManager.Instance.Resolve<GrowlNotifiactions>(),
                IocManager.Instance.Resolve<IStartupConfiguration>());

            this.translator.Initialize();
            var translatorEvents = Observable
                .FromEventPattern(
                    h => this.translator.WhenClipboardContainsTextEventHandler += h,
                    h => this.translator.WhenClipboardContainsTextEventHandler -= h);

            var notifierEvents = Observable
                .FromEventPattern<WhenNotificationAddEventArgs>(
                    h => this.translator.WhenNotificationAddEventHandler += h,
                    h => this.translator.WhenNotificationAddEventHandler -= h);


            translatorEvents.Subscribe(new Finder(this.translator));
            notifierEvents.Subscribe(new Notifier(this.translator));
        }
    }
}