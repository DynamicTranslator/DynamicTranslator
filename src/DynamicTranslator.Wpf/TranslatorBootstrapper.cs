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
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly ICacheManager _cacheManager;
        private readonly IClipboardManager _clipboardManager;
        private readonly GrowlNotifications _growlNotifications;
        private readonly MainWindow _mainWindow;
        private CancellationTokenSource _cancellationTokenSource;
        private IDisposable _finderObservable;
        private IKeyboardMouseEvents _globalMouseHook;
        private IntPtr _hWndNextViewer;
        private HwndSource _hWndSource;
        private bool _isMouseDown;
        private Point _mouseFirstPoint;
        private Point _mouseSecondPoint;
        private IDisposable _syncObserver;

        public TranslatorBootstrapper(MainWindow mainWindow,
            GrowlNotifications growlNotifications,
            IApplicationConfiguration applicationConfiguration,
            ICacheManager cacheManager,
            IClipboardManager clipboardManager)
        {
            _mainWindow = mainWindow;
            _growlNotifications = growlNotifications;
            _applicationConfiguration = applicationConfiguration;
            _cacheManager = cacheManager;
            _clipboardManager = clipboardManager;
        }

        public event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;

        public void Dispose()
        {
            if (IsInitialized)
            {
                if (_cancellationTokenSource.Token.CanBeCanceled)
                {
                    _cancellationTokenSource.Cancel(false);
                }

                DisposeHooks();
                Task.Run(() => SendKeys.Flush());
                UnsubscribeLocalEvents();
                _growlNotifications.Dispose();
                _finderObservable.Dispose();
                _syncObserver.Dispose();
                _cacheManager.GetCache<string, TranslateResult[]>(CacheNames.MeanCache).Clear();
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
            _mainWindow.Dispatcher.ShutdownStarted +=
                (sender, args) => { _cancellationTokenSource?.Cancel(false); };

            _mainWindow.Dispatcher.ShutdownFinished += (sender, args) =>
            {
                Dispose();
                GC.SuppressFinalize(this);
            };
        }

        public bool IsInitialized { get; private set; }

        private static Task SendCopyCommandAsync()
        {
            var thread = new Thread(() =>
            {
                SendKeys.SendWait("^c");
                SendKeys.Flush();
            });
            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();
            thread.Join();

            return Task.FromResult(0);
        }

        private void CompositionRoot()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            StartHooks();
            ConfigureNotificationMeasurements();
            SubscribeLocalevents();
            Task.Run(() => SendKeys.Flush());
            StartObservers();
            IsInitialized = true;
        }

        private void ConfigureNotificationMeasurements()
        {
            _growlNotifications.Top = SystemParameters.WorkArea.Top + _applicationConfiguration.TopOffset;
            _growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - _applicationConfiguration.LeftOffset;
        }

        private void DisposeHooks()
        {
            Win32.ChangeClipboardChain(_hWndSource.Handle, _hWndNextViewer);
            _hWndNextViewer = IntPtr.Zero;
            _hWndSource.RemoveHook(WinProc);
            _globalMouseHook.Dispose();
        }

        private Task HandleTextCaptured(int msg, IntPtr wParam, IntPtr lParam)
        {
            return Task.Run(async () =>
            {
                await _mainWindow.Dispatcher.InvokeAsync(async () =>
                                     {
                                         Win32.SendMessage(_hWndNextViewer, msg, wParam, lParam); //pass the message to the next viewer //clipboard content changed

                                         if (_clipboardManager.IsContainsText())
                                         {
                                             var currentText = _clipboardManager.GetCurrentText();

                                             if (!string.IsNullOrEmpty(currentText))
                                             {
                                                 await TriggerTextCaptured(currentText);
                                                 _clipboardManager.Clear();
                                             }
                                         }
                                     },
                                     DispatcherPriority.Background);
            });
        }

        private async void MouseDoubleClicked(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            await SendCopyCommandAsync();
        }

        private async void MouseDown(object sender, MouseEventArgs e)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            _mouseFirstPoint = e.Location;
            _isMouseDown = true;
            await Task.FromResult(0);
        }

        private async void MouseUp(object sender, MouseEventArgs e)
        {
            if (_isMouseDown && !_mouseSecondPoint.Equals(_mouseFirstPoint))
            {
                _mouseSecondPoint = e.Location;
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }

                await SendCopyCommandAsync();
                _isMouseDown = false;
            }
        }

        private void StartHooks()
        {
            var wih = new WindowInteropHelper(_mainWindow);
            _hWndSource = HwndSource.FromHwnd(wih.Handle);
            _globalMouseHook = Hook.GlobalEvents();
            var source = _hWndSource;
            if (source != null)
            {
                source.AddHook(WinProc); // start processing window messages
                _hWndNextViewer = Win32.SetClipboardViewer(source.Handle); // set this window as a viewer
            }
        }

        private void StartObservers()
        {
            _finderObservable = Observable
                .FromEventPattern<WhenClipboardContainsTextEventArgs>(
                    h => WhenClipboardContainsTextEventHandler += h,
                    h => WhenClipboardContainsTextEventHandler -= h).
                Subscribe(IocManager.Instance.Resolve<Finder>());

            _syncObserver = Observable
                .Interval(TimeSpan.FromSeconds(7.0), TaskPoolScheduler.Default)
                .StartWith(-1L)
                .Subscribe(IocManager.Instance.Resolve<GoogleAnalyticsTracker>());
        }

        private void SubscribeLocalevents()
        {
            _globalMouseHook.MouseDoubleClick += MouseDoubleClicked;
            _globalMouseHook.MouseDown += MouseDown;
            _globalMouseHook.MouseUp += MouseUp;
        }

        private Task TriggerTextCaptured(string currentText)
        {
            return Task.Run(async () =>
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }

                await WhenClipboardContainsTextEventHandler.InvokeSafelyAsync(this,
                    new WhenClipboardContainsTextEventArgs { CurrentString = currentText }
                );
            });
        }

        private void UnsubscribeLocalEvents()
        {
            _globalMouseHook.MouseDoubleClick -= MouseDoubleClicked;
            _globalMouseHook.MouseDownExt -= MouseDown;
            _globalMouseHook.MouseUp -= MouseUp;
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WmChangecbchain:
                    if (wParam == _hWndNextViewer)
                    {
                        _hWndNextViewer = lParam; //clipboard viewer chain changed, need to fix it.
                    }
                    else if (_hWndNextViewer != IntPtr.Zero)
                    {
                        Win32.SendMessage(_hWndNextViewer, msg, wParam, lParam); //pass the message to the next viewer.
                    }
                    break;
                case Win32.WmDrawclipboard:
                    HandleTextCaptured(msg, wParam, lParam).ConfigureAwait(false);
                    break;
            }

            return IntPtr.Zero;
        }
    }
}
