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
        private readonly IStartupConfiguration _configurations;
        private readonly GrowlNotifiactions _growNotifications;
        private CancellationToken cancellationToken;
        private CancellationTokenSource cancellationTokenSource;
        private bool isRunning;
        private ITranslator translator;

        public MainWindow()
        {
            this.InitializeComponent();

            this._configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
            this._growNotifications = IocManager.Instance.Resolve<GrowlNotifiactions>();

            this._growNotifications.Top = SystemParameters.WorkArea.Top + this._configurations.TopOffset;
            this._growNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - this._configurations.LeftOffset;
            this._growNotifications.OnDispose += ClearAllNotifications;
        }

        private static void ClearAllNotifications(object sender, EventArgs args)
        {
            var growl = sender as GrowlNotifiactions;
            if (growl == null) return;
            if (growl.IsDisposed) return;

            growl.Notifications.Clear();
            GC.SuppressFinalize(growl);
            growl.IsDisposed = true;
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
            this.translator = new Translator(this);
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
            notifierEvents.Subscribe(new Notifier(this.translator, this._growNotifications));
        }
    }
}