namespace Dynamic.Translator.Orchestrators
{
    #region

    using System;
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

        public TranslatorBootstrapper(MainWindow mainWindow, GrowlNotifiactions growlNotifications,
            IStartupConfiguration startupConfiguration)
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
            var wih = new WindowInteropHelper(mainWindow);
            hWndSource = HwndSource.FromHwnd(wih.Handle);
            globalMouseHook = Hook.GlobalEvents();
            mainWindow.CancellationTokenSource = new CancellationTokenSource();
            var source = hWndSource;
            if (source != null)
            {
                source.AddHook(WinProc); // start processing window messages
                hWndNextViewer = Win32.SetClipboardViewer(source.Handle); // set this window as a viewer
            }
            SubscribeLocalevents();
            FlushCopyCommand();
            growlNotifications.Top = SystemParameters.WorkArea.Top + startupConfiguration.TopOffset;
            growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width -
                                      startupConfiguration.LeftOffset;
            IsInitialized = true;
        }

        public void Dispose()
        {
            Win32.ChangeClipboardChain(hWndSource.Handle, hWndNextViewer);
            hWndNextViewer = IntPtr.Zero;
            hWndSource.RemoveHook(WinProc);
            IsInitialized = false;
            FlushCopyCommand();
            mainWindow.CancellationTokenSource.Cancel(false);
            UnsubscribeLocalEvents();
            IocManager.Instance.Dispose();
        }

        public bool IsInitialized { get; private set; }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            Task.Run(() =>
            {
                if (isMouseDown && !mouseSecondPoint.Equals(mouseFirstPoint))
                {
                    mouseSecondPoint = e.Location;
                    if (mainWindow.CancellationTokenSource.Token.IsCancellationRequested)
                        return;

                    SendCopyCommand();
                    isMouseDown = false;
                }
            });
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            Task.Run(() =>
            {
                if (mainWindow.CancellationTokenSource.Token.IsCancellationRequested)
                    return;

                mouseFirstPoint = e.Location;
                isMouseDown = true;
            });
        }

        private void MouseDoubleClicked(object sender, MouseEventArgs e)
        {
            Task.Run(() =>
            {
                isMouseDown = false;
                if (mainWindow.CancellationTokenSource.Token.IsCancellationRequested)
                    return;

                SendCopyCommand();
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
                    Win32.SendMessage(hWndNextViewer, msg, wParam, lParam); //pass the message to the next viewer //clipboard content changed
                    if (Clipboard.ContainsText() && !string.IsNullOrEmpty(Clipboard.GetText().Trim()))
                    {
                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Background,
                            (Action) delegate
                            {
                                var currentText = Clipboard.GetText().RemoveSpecialCharacters();

                                if (!string.IsNullOrEmpty(currentText))
                                {
                                    Task.Run(async () =>
                                    {
                                        if (mainWindow.CancellationTokenSource.Token.IsCancellationRequested)
                                            return;

                                        await WhenClipboardContainsTextEventHandler.InvokeSafelyAsync(this, new WhenClipboardContainsTextEventArgs {CurrentString = currentText});

                                        FlushCopyCommand();
                                    });
                                }
                            });
                    }
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

        private void SendCopyCommand()
        {
            SendKeys.SendWait("^c");
            SendKeys.Flush();
        }

        private void FlushCopyCommand()
        {
            SendKeys.Flush();
        }
    }
}