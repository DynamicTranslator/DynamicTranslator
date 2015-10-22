namespace Dynamic.Tureng.Translator
{
    #region using

    using System;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Threading;
    using Dynamic.Translator.Core.Config;
    using Dynamic.Translator.Core.Dependency;
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
        private bool isViewing;

        public MainWindow()
        {
            this.InitializeComponent();

            this._configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
            this._growNotifications = IocManager.Instance.Resolve<GrowlNotifiactions>();

            this._growNotifications.Top = SystemParameters.WorkArea.Top + this._configurations.TopOffset;
            this._growNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - this._configurations.LeftOffset;
            this._growNotifications.OnDispose += ClearAllNotifications;
            Application.Current.DispatcherUnhandledException += this.HandleUnhandledException;
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

        private void HandleUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            //e.Dispatcher.Invoke(() => Application.Current.Run());
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

        private async void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            var translator = new Translator(this);
            var translatorEvents = Observable
                .FromEventPattern(
                    h => translator.WhenClipboardContainsTextEventHandler += h,
                    h => translator.WhenClipboardContainsTextEventHandler -= h);

            var notifierEvents = Observable
                .FromEventPattern<WhenNotificationAddEventArgs>(
                    h => translator.WhenNotificationAddEventHandler += h,
                    h => translator.WhenNotificationAddEventHandler -= h);

            translatorEvents.Subscribe(new Finder(translator));
            notifierEvents.Subscribe(new Notifier(translator, this._growNotifications));


            if (!this.isViewing)
            {
                this.BtnSwitch.Content = "Stop Translator";
            }
            else
            {
                if (this.cancellationToken.CanBeCanceled)
                {
                    this.cancellationTokenSource.Cancel();
                }

                this.BtnSwitch.Content = "Start Translator";
            }
        }


        private void WindowLoaded1(object sender, RoutedEventArgs e)
        {
            // this will make minimize restore of notifications too
            //_growNotifications.Owner = GetWindow(this);
        }


        private void CloseCbViewer()
        {
            //Win32.ChangeClipboardChain(this.hWndSource.Handle, this.hWndNextViewer);
            //this.hWndNextViewer = IntPtr.Zero;
            //this.hWndSource.RemoveHook(this.WinProc);
            //this.RichCurrentText.Document.Blocks.Clear();
            //this.isViewing = false;
        }
    }
}