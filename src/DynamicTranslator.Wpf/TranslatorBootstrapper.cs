using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using DynamicTranslator.Core;
using DynamicTranslator.Core.Configuration;
using DynamicTranslator.ViewModel;
using Gma.System.MouseKeyHook;
using Microsoft.Extensions.DependencyInjection;
using Stateless;

namespace DynamicTranslator
{
    enum State
    {
        Waiting,
        MouseDown,
        TextCaptured,
        DragStarted,
        DragFinished,
        MouseUp
    }

    enum Trigger
    {
        Click,
        DoubleClick,
        ReleaseMouse,
        StartDrag,
        FinishDrag
    }

    //TODO: Check the lifetime of dependencies
    public class TranslatorBootstrapper : IDisposable
    {
        private readonly IClipboardManager _clipboardManager;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly GrowlNotifications _growlNotifications;
        private CancellationTokenSource _cancellationTokenSource;
        private IDisposable _finderObservable;
        private readonly IKeyboardMouseEvents _globalMouseHook;
        private IDisposable _syncObserver;
        private readonly IGoogleAnalyticsTracker _googleAnalyticsTracker;
        private readonly IServiceProvider _serviceProvider;
        private StateMachine<State, Trigger> _stateMachine;

        public TranslatorBootstrapper(GrowlNotifications growlNotifications,
            IClipboardManager clipboardManager,
            IApplicationConfiguration applicationConfiguration,
            IGoogleAnalyticsTracker googleAnalyticsTracker,
            IServiceProvider serviceProvider)
        {
            _growlNotifications = growlNotifications;
            _clipboardManager = clipboardManager;
            _applicationConfiguration = applicationConfiguration;
            _googleAnalyticsTracker = googleAnalyticsTracker;
            _serviceProvider = serviceProvider;
            _globalMouseHook = Hook.GlobalEvents();

            ConfigureStateMachine();
            ConfigureNotificationMeasurements();
        }

        void ConfigureStateMachine()
        {
            _stateMachine = new StateMachine<State, Trigger>(State.Waiting);

            _stateMachine.Configure(State.Waiting)
                .Permit(Trigger.DoubleClick, State.TextCaptured)
                .Permit(Trigger.StartDrag, State.DragStarted)
                .PermitReentry(Trigger.ReleaseMouse)
                ;

            _stateMachine.Configure(State.DragStarted)
                .Permit(Trigger.FinishDrag, State.TextCaptured)
                .Permit(Trigger.ReleaseMouse, State.Waiting)
                ;

            _stateMachine.Configure(State.TextCaptured)
                .Permit(Trigger.ReleaseMouse, State.Waiting)
                .Ignore(Trigger.DoubleClick)
                .Ignore(Trigger.StartDrag)
                .Ignore(Trigger.FinishDrag)
                .OnEntry(SendCopyCommand)
                ;
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
            _globalMouseHook.Dispose();
            IsInitialized = false;
        }

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            SubscribeLocalEvents();
            SendKeys.Flush();
            StartObservers();
            IsInitialized = true;
        }

        public bool IsInitialized { get; private set; }

        void ConfigureNotificationMeasurements()
        {
            _growlNotifications.Top = SystemParameters.WorkArea.Top + _applicationConfiguration.TopOffset;
            _growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - _applicationConfiguration.LeftOffset;
        }

        void StartObservers()
        {
            _finderObservable = Observable
                .FromEventPattern<WhenClipboardContainsTextEventArgs>(
                    h => WhenClipboardContainsTextEventHandler += h,
                    h => WhenClipboardContainsTextEventHandler -= h)
                .Select(pattern => Observable.FromAsync(token => _serviceProvider.GetRequiredService<IFinder>().Find(pattern.EventArgs.CurrentString, token)))
                .Concat()
                .Subscribe();

            _syncObserver = Observable
                .Interval(TimeSpan.FromSeconds(7.0), TaskPoolScheduler.Default)
                .StartWith(-1L)
                .Select((l, i) => _googleAnalyticsTracker.Track())
                .Subscribe();
        }

        void SubscribeLocalEvents()
        {
            _globalMouseHook.MouseDoubleClick += MouseDoubleClicked;
            _globalMouseHook.MouseDragStarted += MouseDragStarted;
            _globalMouseHook.MouseDragFinished += MouseDragFinished;
        }

        void UnsubscribeLocalEvents()
        {
            _globalMouseHook.MouseDoubleClick -= MouseDoubleClicked;
            _globalMouseHook.MouseDragStarted -= MouseDragStarted;
            _globalMouseHook.MouseDragFinished -= MouseDragFinished;
        }

        void MouseDoubleClicked(object sender, MouseEventArgs e)
        {
            lock (_stateMachine)
            {
                if (!_stateMachine.IsInState(State.Waiting))
                {
                    _stateMachine.Fire(Trigger.ReleaseMouse);
                    return;
                }

                _stateMachine.Fire(Trigger.DoubleClick);
            }
            
        }

        void MouseDragStarted(object sender, MouseEventArgs e)
        {
            lock (_stateMachine)
            {
                if (!_stateMachine.IsInState(State.Waiting))
                {
                    _stateMachine.Fire(Trigger.ReleaseMouse);
                    return;
                }

                _stateMachine.Fire(Trigger.StartDrag);
            }
        }

        void MouseDragFinished(object sender, MouseEventArgs e)
        {
            lock (_stateMachine)
            {
                if (!_stateMachine.IsInState(State.DragStarted))
                {
                    _stateMachine.Fire(Trigger.ReleaseMouse);
                    return;
                }

                _stateMachine.Fire(Trigger.FinishDrag);
            }
        }

        void SendCopyCommand()
        {
            SendKeys.SendWait("^c");

            if (!_clipboardManager.ContainsText()) return;
            
            var currentText = _clipboardManager.GetCurrentText();

            if (!string.IsNullOrEmpty(currentText))
            {
                TextCaptured(currentText);
                _clipboardManager.Clear();
            }

            lock (_stateMachine)
            {
                _stateMachine.Fire(Trigger.ReleaseMouse);
            }
        }

        void TextCaptured(string currentText)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested) return;

            WhenClipboardContainsTextEventHandler?.Invoke(this,
               new WhenClipboardContainsTextEventArgs { CurrentString = currentText }
           );
        }
    }
}