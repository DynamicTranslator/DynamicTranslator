namespace DynamicTranslator
{
    using System;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Threading;
    using Core;
    using Core.Configuration;
    using Gma.System.MouseKeyHook;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModel;
    using Notification = ViewModel.Notification;

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
                this.serviceProvider
                    .GetRequiredService<MainWindow>()
                    .Dispatcher
                    .InvokeAsync(SendCopyCommand, DispatcherPriority.Input, this.cancellationTokenSource.Token);
                this.tipsyMouse.Release();
            });
            ConfigureNotificationMeasurements();
        }

        public bool IsInitialized { get; private set; }

        public void Dispose()
        {
            this.cancellationTokenSource.Cancel(false);

            UnsubscribeMouseHook();
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
            SubscribeMouseHook();
            SendKeys.Flush();
            StartObservers();
            InitializeCookies();
            IsInitialized = true;
        }

        public void Stop()
        {
            IsInitialized = false;
            UnsubscribeMouseHook();
            this.finderObservable.Dispose();
            this.syncObserver.Dispose();
            this.cancellationTokenSource.Cancel();
        }

        void InitializeCookies()
        {
        }

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
                .Select(pattern => Observable.FromAsync(token => ObserveWithHandlingException(pattern, token)))
                .Concat()
                .Subscribe();

            this.syncObserver = Observable
                .Interval(TimeSpan.FromSeconds(7.0), TaskPoolScheduler.Default)
                .StartWith(-1L)
                .Select((l, i) => this.googleAnalyticsTracker.Track())
                .Subscribe();
        }

        Task ObserveWithHandlingException(EventPattern<WhenClipboardContainsTextEventArgs> pattern,
            CancellationToken token)
        {
            try
            {
                return this.serviceProvider
                    .GetRequiredService<IFinder>()
                    .Find(pattern.EventArgs.CurrentString, token);
            }
            catch (Exception)
            {
                this.serviceProvider
                    .GetRequiredService<GrowlNotifications>()
                    .AddNotification(new Notification { Title = "Error", Message = "An unhandled exception occurred!" });
            }

            return Task.CompletedTask;
        }

        void SubscribeMouseHook()
        {
            this.globalMouseHook.MouseDoubleClick += MouseDoubleClicked;
            this.globalMouseHook.MouseDragStarted += MouseDragStarted;
            this.globalMouseHook.MouseDragFinished += MouseDragFinished;
        }

        void UnsubscribeMouseHook()
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
                new WhenClipboardContainsTextEventArgs { CurrentString = currentText }
            );
        }
    }
}