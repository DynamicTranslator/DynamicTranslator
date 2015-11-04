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
    using Core.Extensions;
    using Core.Orchestrators;
    using Gma.System.MouseKeyHook;
    using Utility;
    using ViewModel;
    using Application = System.Windows.Application;
    using Clipboard = System.Windows.Clipboard;
    using Point = System.Drawing.Point;

    #endregion

    public class TranslatorBootstrapper : ITranslatorBootstrapper
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
            var wih = new WindowInteropHelper(this.mainWindow);
            this.hWndSource = HwndSource.FromHwnd(wih.Handle);
            this.globalMouseHook = Hook.GlobalEvents();
            var source = this.hWndSource;
            if (source != null)
            {
                source.AddHook(this.WinProc); // start processing window messages
                this.hWndNextViewer = Win32.SetClipboardViewer(source.Handle); // set this window as a viewer
            }
            this.globalMouseHook.MouseDoubleClick += async (o, args) => await this.MouseDoubleClicked(o, args);
            this.globalMouseHook.MouseDown += async (o, args) => await this.MouseDown(o, args);
            this.globalMouseHook.MouseUp += async (o, args) => await this.MouseUp(o, args);
            this.growlNotifications.OnDispose += this.ClearAllNotifications;
            this.growlNotifications.Top = SystemParameters.WorkArea.Top + this.startupConfiguration.TopOffset;
            this.growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - this.startupConfiguration.LeftOffset;
            this.IsInitialized = true;
        }

        public void Dispose()
        {
            Win32.ChangeClipboardChain(this.hWndSource.Handle, this.hWndNextViewer);
            this.hWndNextViewer = IntPtr.Zero;
            this.hWndSource.RemoveHook(this.WinProc);
            this.IsInitialized = false;
            this.mainWindow.CancellationTokenSource.Cancel(false);
            this.growlNotifications.OnDispose -= this.ClearAllNotifications;
            this.globalMouseHook.MouseDoubleClick -= (async (o, args) => await this.MouseDoubleClicked(o, args));
            this.globalMouseHook.MouseDownExt -= (async (o, args) => await this.MouseDown(o, args));
            this.globalMouseHook.MouseUp -= (async (o, args) => await this.MouseUp(o, args));
        }

        public bool IsInitialized { get; private set; }

        private async Task MouseUp(object sender, MouseEventArgs e)
        {
            this.mouseSecondPoint = e.Location;

            if (this.isMouseDown && !this.mouseSecondPoint.Equals(this.mouseFirstPoint))
            {
                await Task.Run(() => { SendKeys.SendWait("^c"); },
                    this.mainWindow.CancellationTokenSource.Token);
                this.isMouseDown = false;
            }
            this.isMouseDown = false;
        }

        private async Task MouseDown(object sender, MouseEventArgs e)
        {
            await Task.Run(() =>
            {
                this.mouseFirstPoint = e.Location;
                this.isMouseDown = true;
            },
                this.mainWindow.CancellationTokenSource.Token);
        }

        private async Task MouseDoubleClicked(object sender, MouseEventArgs e)
        {
            this.isMouseDown = false;
            await Task.Run(() => { SendKeys.SendWait("^c"); },
                this.mainWindow.CancellationTokenSource.Token);
        }

        private void ClearAllNotifications(object sender, EventArgs args)
        {
            var growl = sender as GrowlNotifiactions;
            if (growl == null)
                return;
            if (growl.IsDisposed)
                return;

            growl.Notifications.Clear();
            growl.IsDisposed = true;
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WmChangecbchain:
                    if (wParam == this.hWndNextViewer)
                        this.hWndNextViewer = lParam; //clipboard viewer chain changed, need to fix it.
                    else if (this.hWndNextViewer != IntPtr.Zero)
                        Win32.SendMessage(this.hWndNextViewer, msg, wParam, lParam); //pass the message to the next viewer.

                    break;
                case Win32.WmDrawclipboard:
                    Win32.SendMessage(this.hWndNextViewer, msg, wParam, lParam); //pass the message to the next viewer //clipboard content changed
                    if (Clipboard.ContainsText() && !string.IsNullOrEmpty(Clipboard.GetText().Trim()))
                    {
                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Background,
                            (Action)
                                delegate
                                {
                                    var currentText = Clipboard.GetText().RemoveSpecialCharacters();

                                    if (!string.IsNullOrEmpty(currentText))
                                    {
                                        Task.Run(
                                            async () =>
                                                await
                                                    this.WhenClipboardContainsTextEventHandler.InvokeSafelyAsync(this,
                                                        new WhenClipboardContainsTextEventArgs {CurrentString = currentText}),
                                            this.mainWindow.CancellationTokenSource.Token);
                                    }
                                });
                    }
                    break;
            }

            return IntPtr.Zero;
        }
    }
}