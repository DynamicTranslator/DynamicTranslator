using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;

using Abp.Dependency;
using Abp.Runtime.Caching;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Events;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;
using DynamicTranslator.Wpf.Observers;
using DynamicTranslator.Wpf.ViewModel;

using Gma.System.MouseKeyHook;

using Point = System.Drawing.Point;

namespace DynamicTranslator.Wpf
{
    public class TranslatorBootstrapper : ITranslatorBootstrapper, ISingletonDependency
    {
        private readonly IApplicationConfiguration applicationConfiguration;

        private readonly ITypedCache<string, TranslateResult[]> cache;
        private readonly ICacheManager cacheManager;
        private readonly IClipboardManager clipboardManager;
        private readonly GrowlNotifications growlNotifications;
        private readonly MainWindow mainWindow;
        private CancellationTokenSource cancellationTokenSource;
        private IDisposable finderObservable;
        private IKeyboardMouseEvents globalMouseHook;
        private IntPtr hWndNextViewer;
        private HwndSource hWndSource;
        private bool isMouseDown;
        private Point mouseFirstPoint;
        private Point mouseSecondPoint;
        private IDisposable syncObserver;

        public TranslatorBootstrapper(MainWindow mainWindow,
            GrowlNotifications growlNotifications,
            IApplicationConfiguration applicationConfiguration,
            ICacheManager cacheManager,
            IClipboardManager clipboardManager)
        {
            this.mainWindow = mainWindow;
            this.growlNotifications = growlNotifications;
            this.applicationConfiguration = applicationConfiguration;
            this.cacheManager = cacheManager;
            this.clipboardManager = clipboardManager;
            cache = this.cacheManager.GetCache<string, TranslateResult[]>(CacheNames.MeanCache);
        }

        public event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;

        public void Dispose()
        {
            if (IsInitialized)
            {
                if (cancellationTokenSource.Token.CanBeCanceled)
                {
                    cancellationTokenSource.Cancel(false);
                }

                DisposeHooks();
                Task.Run(() => SendKeys.Flush());
                UnsubscribeLocalEvents();
                growlNotifications.Dispose();
                finderObservable.Dispose();
                syncObserver.Dispose();
                cache.Clear();
                IsInitialized = false;
            }
        }

        public void Initialize()
        {
            CompositionRoot();
        }

        public Task InitializeAsync()
        {
            Initialize();
            return Task.FromResult(0);
        }

        public void SubscribeShutdownEvents()
        {
            mainWindow.Dispatcher.ShutdownStarted +=
                (sender, args) => { cancellationTokenSource?.Cancel(false); };

            mainWindow.Dispatcher.ShutdownFinished += (sender, args) =>
            {
                Dispose();
                GC.SuppressFinalize(this);
            };
        }

        public bool IsInitialized { get; private set; }

        private static Task SendCopyCommandAsync()
        {
            SendKeys.SendWait("^c");
            SendKeys.Flush();
            return Task.FromResult(0);
        }

        private void CompositionRoot()
        {
            cancellationTokenSource = new CancellationTokenSource();
            StartHooks();
            ConfigureNotificationMeasurements();
            SubscribeLocalevents();
            Task.Run(() => SendKeys.Flush());
            StartObservers();
            IsInitialized = true;
        }

        private void ConfigureNotificationMeasurements()
        {
            growlNotifications.Top = SystemParameters.WorkArea.Top + applicationConfiguration.TopOffset;
            growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - applicationConfiguration.LeftOffset;
        }

        private void DisposeHooks()
        {
            Win32.ChangeClipboardChain(hWndSource.Handle, hWndNextViewer);
            hWndNextViewer = IntPtr.Zero;
            hWndSource.RemoveHook(WinProc);
            globalMouseHook.Dispose();
        }

        private void HandleTextCaptured(int msg, IntPtr wParam, IntPtr lParam)
        {
            Task.Run(async () =>
            {
                await mainWindow.Dispatcher.InvokeAsync(async () =>
                {
                    Win32.SendMessage(hWndNextViewer, msg, wParam, lParam); //pass the message to the next viewer //clipboard content changed

                    if (clipboardManager.IsContainsText())
                    {
                        var currentText = clipboardManager.GetCurrentText();

                        if (!string.IsNullOrEmpty(currentText))
                        {
                            await TriggerTextCaptured(currentText);
                        }
                    }
                },
                    DispatcherPriority.Background);
            });
        }

        private async void MouseDoubleClicked(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            if (cancellationTokenSource.Token.IsCancellationRequested)
                return;

            await SendCopyCommandAsync();
        }

        private async void MouseDown(object sender, MouseEventArgs e)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
                return;

            mouseFirstPoint = e.Location;
            isMouseDown = true;
            await Task.FromResult(0);
        }

        private async void MouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseDown && !mouseSecondPoint.Equals(mouseFirstPoint))
            {
                mouseSecondPoint = e.Location;
                if (cancellationTokenSource.Token.IsCancellationRequested)
                    return;

                await SendCopyCommandAsync();
                isMouseDown = false;
            }
        }

        private void StartHooks()
        {
            var wih = new WindowInteropHelper(mainWindow);
            hWndSource = HwndSource.FromHwnd(wih.Handle);
            globalMouseHook = Hook.GlobalEvents();
            var source = hWndSource;
            if (source != null)
            {
                source.AddHook(WinProc); // start processing window messages
                hWndNextViewer = Win32.SetClipboardViewer(source.Handle); // set this window as a viewer
            }
        }

        private void StartObservers()
        {
            finderObservable = Observable
                .FromEventPattern<WhenClipboardContainsTextEventArgs>(
                    h => WhenClipboardContainsTextEventHandler += h,
                    h => WhenClipboardContainsTextEventHandler -= h).
                 Subscribe(IocManager.Instance.Resolve<Finder>());

            syncObserver = Observable
                .Interval(TimeSpan.FromSeconds(7.0), TaskPoolScheduler.Default)
                .StartWith(-1L)
                .Subscribe(IocManager.Instance.Resolve<Feeder>());
        }

        private void SubscribeLocalevents()
        {
            globalMouseHook.MouseDoubleClick += MouseDoubleClicked;
            globalMouseHook.MouseDown += MouseDown;
            globalMouseHook.MouseUp += MouseUp;
        }

        private Task TriggerTextCaptured(string currentText)
        {
            return Task.Run(async () =>
            {
                if (cancellationTokenSource.Token.IsCancellationRequested)
                    return;

                await WhenClipboardContainsTextEventHandler.InvokeSafelyAsync(this,
                    new WhenClipboardContainsTextEventArgs {CurrentString = currentText}
                    );

                SendKeys.Flush();
            });
        }

        private void UnsubscribeLocalEvents()
        {
            globalMouseHook.MouseDoubleClick -= MouseDoubleClicked;
            globalMouseHook.MouseDownExt -= MouseDown;
            globalMouseHook.MouseUp -= MouseUp;
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WmChangecbchain:
                    if (wParam == hWndNextViewer)
                        hWndNextViewer = lParam; //clipboard viewer chain changed, need to fix it.
                    else if (hWndNextViewer != IntPtr.Zero)
                        Win32.SendMessage(hWndNextViewer, msg, wParam, lParam); //pass the message to the next viewer.
                    break;
                case Win32.WmDrawclipboard:
                    HandleTextCaptured(msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }
    }
}