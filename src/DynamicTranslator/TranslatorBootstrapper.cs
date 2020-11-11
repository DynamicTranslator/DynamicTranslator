namespace DynamicTranslator
{
    using System;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Forms;
    using Core;
    using Core.Configuration;
    using Gma.System.MouseKeyHook;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModel;

    //TODO: Check the lifetime of dependencies
    public class TranslatorBootstrapper : IDisposable
    {
        readonly IApplicationConfiguration applicationConfiguration;
        readonly IClipboardManager clipboardManager;
        readonly CookieContainer cookieContainer;
        readonly IKeyboardMouseEvents globalMouseHook;
        readonly IGoogleAnalyticsTracker googleAnalyticsTracker;
        readonly GrowlNotifications growlNotifications;
        readonly IServiceProvider serviceProvider;
        readonly TipsyMouse tipsyMouse;
        CancellationTokenSource cancellationTokenSource;
        IDisposable finderObservable;
        IDisposable syncObserver;

        public TranslatorBootstrapper(GrowlNotifications growlNotifications,
            IClipboardManager clipboardManager,
            IApplicationConfiguration applicationConfiguration,
            IGoogleAnalyticsTracker googleAnalyticsTracker,
            IServiceProvider serviceProvider,
            CookieContainer cookieContainer)
        {
            this.growlNotifications = growlNotifications;
            this.clipboardManager = clipboardManager;
            this.applicationConfiguration = applicationConfiguration;
            this.googleAnalyticsTracker = googleAnalyticsTracker;
            this.serviceProvider = serviceProvider;
            this.cookieContainer = cookieContainer;
            this.globalMouseHook = Hook.GlobalEvents();
            this.tipsyMouse = new TipsyMouse(() =>
            {
                SendCopyCommand();
                this.tipsyMouse.Release();
            });
            ConfigureNotificationMeasurements();
        }

        public bool IsInitialized { get; private set; }

        public void Dispose()
        {
            if (!IsInitialized) return;

            if (this.cancellationTokenSource.Token.CanBeCanceled) this.cancellationTokenSource.Cancel(false);

            UnsubscribeLocalEvents();
            this.growlNotifications.Dispose();
            this.finderObservable.Dispose();
            this.syncObserver.Dispose();
            this.globalMouseHook.Dispose();
            IsInitialized = false;
        }

        public event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;

        public void Initialize()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            SubscribeLocalEvents();
            SendKeys.Flush();
            StartObservers();
            InitializeCookies();
            IsInitialized = true;
        }

        void InitializeCookies() { }


        void ConfigureNotificationMeasurements()
        {
            this.growlNotifications.Top = SystemParameters.WorkArea.Top + this.applicationConfiguration.TopOffset;
            this.growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width -
                                           this.applicationConfiguration.LeftOffset;
        }

        void StartObservers()
        {
            this.finderObservable = Observable
                .FromEventPattern<WhenClipboardContainsTextEventArgs>(
                    h => WhenClipboardContainsTextEventHandler += h,
                    h => WhenClipboardContainsTextEventHandler -= h)
                .Select(pattern => Observable.FromAsync(token =>
                {
                    try
                    {
                        return this.serviceProvider.GetRequiredService<IFinder>()
                            .Find(pattern.EventArgs.CurrentString, token);
                    }
                    catch (Exception)
                    {
                        this.serviceProvider.GetRequiredService<GrowlNotifications>().AddNotification(
                            new Notification {Title = "Error", Message = "An unhandled exception occurred!"});
                    }

                    return Task.CompletedTask;
                }))
                .Concat()
                .Subscribe();

            this.syncObserver = Observable
                .Interval(TimeSpan.FromSeconds(7.0), TaskPoolScheduler.Default)
                .StartWith(-1L)
                .Select((l, i) => this.googleAnalyticsTracker.Track())
                .Subscribe();
        }

        void SubscribeLocalEvents()
        {
            this.globalMouseHook.MouseDoubleClick += MouseDoubleClicked;
            this.globalMouseHook.MouseDragStarted += MouseDragStarted;
            this.globalMouseHook.MouseDragFinished += MouseDragFinished;
        }

        void UnsubscribeLocalEvents()
        {
            this.globalMouseHook.MouseDoubleClick -= MouseDoubleClicked;
            this.globalMouseHook.MouseDragStarted -= MouseDragStarted;
            this.globalMouseHook.MouseDragFinished -= MouseDragFinished;
        }

        void MouseDoubleClicked(object sender, MouseEventArgs e)
        {
            this.tipsyMouse.DoubleClick();
        }

        void MouseDragStarted(object sender, MouseEventArgs e)
        {
            this.tipsyMouse.StartDragging();
        }

        void MouseDragFinished(object sender, MouseEventArgs e)
        {
            this.tipsyMouse.FinishDragging();
        }

        void SendCopyCommand()
        {
            SendKeys.SendWait("^c");

            if (!this.clipboardManager.ContainsText()) return;

            string currentText = this.clipboardManager.GetCurrentText();

            if (!string.IsNullOrEmpty(currentText))
            {
                TextCaptured(currentText);
                this.clipboardManager.Clear();
            }
        }

        void TextCaptured(string currentText)
        {
            if (this.cancellationTokenSource.Token.IsCancellationRequested) return;

            WhenClipboardContainsTextEventHandler?.Invoke(this,
                new WhenClipboardContainsTextEventArgs {CurrentString = currentText}
            );
        }
    }
}