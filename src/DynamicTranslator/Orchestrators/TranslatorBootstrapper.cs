namespace DynamicTranslator.Orchestrators
{
    #region using

    using System;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Interop;
    using System.Windows.Threading;
    using Core.Config;
    using Core.Dependency.Manager;
    using Core.Dependency.Markers;
    using Core.Extensions;
    using Core.Orchestrators;
    using Gma.System.MouseKeyHook;
    using Observers;
    using Utility;
    using ViewModel;
    using Application = System.Windows.Application;
    using Clipboard = System.Windows.Clipboard;
    using Point = System.Drawing.Point;

    #endregion

    public class TranslatorBootstrapper : ITranslatorBootstrapper, ISingletonDependency
    {
        private readonly GrowlNotifiactions growlNotifications;
        private readonly MainWindow mainWindow;
        private readonly IStartupConfiguration startupConfiguration;
        private IKeyboardMouseEvents globalMouseHook;
        private IntPtr hWndNextViewer;
        private HwndSource hWndSource;
        private bool isMouseDown;
        private Point mouseFirstPoint;
        private Point mouseSecondPoint;
        private IDisposable finderObservable;

        public TranslatorBootstrapper(MainWindow mainWindow, GrowlNotifiactions growlNotifications, IStartupConfiguration startupConfiguration)
        {
            if (mainWindow == null)
                throw new ArgumentNullException(nameof(mainWindow));

            if (growlNotifications == null)
                throw new ArgumentNullException(nameof(growlNotifications));

            if (startupConfiguration == null)
                throw new ArgumentNullException(nameof(startupConfiguration));

            this.mainWindow = mainWindow;
            this.growlNotifications = growlNotifications;
            this.startupConfiguration = startupConfiguration;
        }

        public event EventHandler<WhenClipboardContainsTextEventArgs> WhenClipboardContainsTextEventHandler;

        public void Initialize()
        {
            Task.Run(async () => await CompositionRoot());
        }

        public void Dispose()
        {
            Task.Run(async () => await DecomposeRoot());
        }

        public bool IsInitialized { get; private set; }

        private async Task CompositionRoot()
        {
            await Task.Run(() =>
            {
                mainWindow.Dispatcher.InvokeAsync(() =>
                {
                    mainWindow.CancellationTokenSource = new CancellationTokenSource();
                    StartHooks();
                    ConfigureNotificationMeasurements();
                    SubscribeLocalevents();
                    FlushCopyCommand();
                    StartObservers();
                    IsInitialized = true;
                });
            });
        }

        private async Task DecomposeRoot()
        {
            await Task.Run(() =>
            {
                mainWindow.Dispatcher.InvokeAsync(() =>
                {
                    mainWindow.CancellationTokenSource.Cancel(false);
                    DisposeHooks();
                    FlushCopyCommand();
                    UnsubscribeLocalEvents();
                    growlNotifications.Dispose();
                    finderObservable.Dispose();
                    IsInitialized = false;
                });
            });
        }

        private void DisposeHooks()
        {
            Win32.ChangeClipboardChain(hWndSource.Handle, hWndNextViewer);
            hWndNextViewer = IntPtr.Zero;
            hWndSource.RemoveHook(WinProc);
            globalMouseHook.Dispose();
        }

        private void ConfigureNotificationMeasurements()
        {
            growlNotifications.Top = SystemParameters.WorkArea.Top + startupConfiguration.TopOffset;
            growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - startupConfiguration.LeftOffset;
        }

        private void StartHooks()
        {
            var wih = new WindowInteropHelper(mainWindow);
            hWndSource = HwndSource.FromHwnd(wih.Handle);
            globalMouseHook = Hook.GlobalEvents();
            var source = hWndSource;
            if (source != null)
            {
                source?.AddHook(WinProc); // start processing window messages
                hWndNextViewer = Win32.SetClipboardViewer(source.Handle); // set this window as a viewer
            }
        }

        private void StartObservers()
        {
            Task.Run(() =>
            {
                finderObservable = Observable
                    .FromEventPattern<WhenClipboardContainsTextEventArgs>(
                        h => WhenClipboardContainsTextEventHandler += h,
                        h => WhenClipboardContainsTextEventHandler -= h)
                    .Subscribe(IocManager.Instance.Resolve<Finder>());
            });
        }

        private async void MouseUp(object sender, MouseEventArgs e)
        {
            await Task.Run(async () =>
            {
                if (isMouseDown && !mouseSecondPoint.Equals(mouseFirstPoint))
                {
                    mouseSecondPoint = e.Location;
                    if (mainWindow.CancellationTokenSource.Token.IsCancellationRequested)
                        return;

                    await SendCopyCommandAsync();
                    isMouseDown = false;
                }
            });
        }

        private async void MouseDown(object sender, MouseEventArgs e)
        {
            await Task.Run(() =>
            {
                if (mainWindow.CancellationTokenSource.Token.IsCancellationRequested)
                    return;

                mouseFirstPoint = e.Location;
                isMouseDown = true;
            });
        }

        private async void MouseDoubleClicked(object sender, MouseEventArgs e)
        {
            await Task.Run(async () =>
            {
                isMouseDown = false;
                if (mainWindow.CancellationTokenSource.Token.IsCancellationRequested)
                    return;

                await SendCopyCommandAsync();
            });
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
                    Application.Current.Dispatcher.InvokeAsync(
                        delegate
                        {
                            Win32.SendMessage(hWndNextViewer, msg, wParam, lParam); //pass the message to the next viewer //clipboard content changed
                            if (Clipboard.ContainsText() && !string.IsNullOrEmpty(Clipboard.GetText().Trim()))
                            {
                                var currentText = Clipboard.GetText().RemoveSpecialCharacters();

                                if (!string.IsNullOrEmpty(currentText))
                                {
                                    Task.Run(async () =>
                                    {
                                        if (mainWindow.CancellationTokenSource.Token.IsCancellationRequested)
                                            return;

                                        WhenClipboardContainsTextEventHandler.InvokeSafelyAsync(this, new WhenClipboardContainsTextEventArgs { CurrentString = currentText });

                                        await FlushCopyCommandAsync();
                                    });
                                }
                            }
                        }, DispatcherPriority.Background);

                    break;
            }

            return IntPtr.Zero;
        }

        private void SubscribeLocalevents()
        {
            globalMouseHook.MouseDoubleClick += MouseDoubleClicked;
            globalMouseHook.MouseDown += MouseDown;
            globalMouseHook.MouseUp += MouseUp;
        }

        private void UnsubscribeLocalEvents()
        {
            globalMouseHook.MouseDoubleClick -= MouseDoubleClicked;
            globalMouseHook.MouseDownExt -= MouseDown;
            globalMouseHook.MouseUp -= MouseUp;
        }

        private async Task SendCopyCommandAsync()
        {
            await Task.Run(() =>
            {
                SendKeys.SendWait("^c");
                SendKeys.Flush();
            });
        }

        private async Task FlushCopyCommandAsync()
        {
            await Task.Run(() => { SendKeys.Flush(); });
        }

        private void FlushCopyCommand()
        {
            Task.Run(() => { SendKeys.Flush(); });
        }
    }
}