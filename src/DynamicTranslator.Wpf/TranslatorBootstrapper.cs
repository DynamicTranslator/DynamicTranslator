using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using DynamicTranslator.Wpf.ViewModel;
using Gma.System.MouseKeyHook;
using System.Reactive.Concurrency;
using DynamicTranslator.Configuration;
using DynamicTranslator.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Point = System.Drawing.Point;

namespace DynamicTranslator.Wpf
{
    //TODO: Check the lifetime of dependencies
    public class TranslatorBootstrapper : IDisposable
    {
        private readonly ClipboardManager _clipboardManager;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly GrowlNotifications _growlNotifications;
        private CancellationTokenSource _cancellationTokenSource;
        private IDisposable _finderObservable;
        private readonly IKeyboardMouseEvents _globalMouseHook;
        private Point _mouseFirstPoint;
        private Point _mouseSecondPoint;
        private IDisposable _syncObserver;
        private readonly InterlockedBoolean _isMouseDown = new InterlockedBoolean();
        private readonly IFinder _finder;
        private readonly IGoogleAnalyticsTracker _googleAnalyticsTracker;
        private readonly IServiceProvider _serviceProvider;

        public TranslatorBootstrapper(GrowlNotifications growlNotifications,
            ClipboardManager clipboardManager,
            IApplicationConfiguration applicationConfiguration,
            IFinder finder,
            IGoogleAnalyticsTracker googleAnalyticsTracker, 
            IServiceProvider serviceProvider)
        {
            _growlNotifications = growlNotifications;
            _clipboardManager = clipboardManager;
            _applicationConfiguration = applicationConfiguration;
            _finder = finder;
            _googleAnalyticsTracker = googleAnalyticsTracker;
            _serviceProvider = serviceProvider;
            _globalMouseHook = Hook.GlobalEvents();
            ConfigureNotificationMeasurements();
        }

        public event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;

        public void Dispose()
        {
            if (!IsInitialized)
            {
                return;
            }

            if (_cancellationTokenSource.Token.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel(false);
            }

            UnsubscribeLocalEvents();
            _growlNotifications.Dispose();
            _finderObservable.Dispose();
            _syncObserver.Dispose();
            IsInitialized = false;
        }

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            SubscribeLocalEvents();
            Task.Run(SendKeys.Flush);
            StartObservers();
            IsInitialized = true;
        }

        public bool IsInitialized { get; private set; }

        private void ConfigureNotificationMeasurements()
        {
            _growlNotifications.Top = SystemParameters.WorkArea.Top + _applicationConfiguration.TopOffset;
            _growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - _applicationConfiguration.LeftOffset;
        }

        private void DisposeHooks()
        {
            _globalMouseHook.Dispose();
        }

        private void StartObservers()
        {
            _finderObservable = Observable
                .FromEventPattern<WhenClipboardContainsTextEventArgs>(
                    h => WhenClipboardContainsTextEventHandler += h,
                    h => WhenClipboardContainsTextEventHandler -= h)
                .Select(pattern => Observable.FromAsync(token => _serviceProvider.GetService<IFinder>().Find(pattern.EventArgs.CurrentString, token)))
                .Concat()
                .Subscribe();

            _syncObserver = Observable
                .Interval(TimeSpan.FromSeconds(7.0), TaskPoolScheduler.Default)
                .StartWith(-1L)
                .Select((l, i) => _googleAnalyticsTracker.Track())
                .Subscribe();
        }

        private void SubscribeLocalEvents()
        {
            _globalMouseHook.MouseDoubleClick += MouseDoubleClicked;
            _globalMouseHook.MouseDragStarted += MouseDragStarted;
            _globalMouseHook.MouseDragFinished += MouseDragFinished;
        }
        private void MouseDoubleClicked(object sender, MouseEventArgs e)
        {
            _isMouseDown.Set(false);

            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            SendCopyCommand();
        }

        private void MouseDragStarted(object sender, MouseEventArgs e)
        {
            _isMouseDown.Set(true);
        }

        private void MouseDragFinished(object sender, MouseEventArgs e)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested) return;
            _isMouseDown.Set(false);
            SendCopyCommand();
        }

        private void SendCopyCommand()
        {
            SendKeys.SendWait("^c");
            SendKeys.Flush();

            if (!_clipboardManager.ContainsText()) return;

            var currentText = _clipboardManager.GetCurrentText();

            if (!string.IsNullOrEmpty(currentText))
            {
                TextCaptured(currentText);
                _clipboardManager.Clear();
            }
        }

        private void TextCaptured(string currentText)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested) return;

            WhenClipboardContainsTextEventHandler?.Invoke(this,
               new WhenClipboardContainsTextEventArgs { CurrentString = currentText }
           );
        }

        private void UnsubscribeLocalEvents()
        {
            _globalMouseHook.MouseDoubleClick -= MouseDoubleClicked;
            _globalMouseHook.MouseDownExt -= MouseDown;
            _globalMouseHook.MouseUp -= MouseUp;
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested) return;

            if ((e.Button & MouseButtons.Left) != 0)
            {
                _isMouseDown.Set(true);
                _mouseFirstPoint = e.Location;
            }
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {
                if (_isMouseDown.Value && !(_mouseFirstPoint.X == _mouseSecondPoint.X && _mouseFirstPoint.Y == _mouseSecondPoint.Y))
                {
                    _isMouseDown.Set(false);
                    _mouseSecondPoint = e.Location;
                    if (_cancellationTokenSource.Token.IsCancellationRequested) return;

                    SendCopyCommand();
                }
            }
        }
    }
}