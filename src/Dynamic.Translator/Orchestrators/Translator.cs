namespace Dynamic.Translator.Orchestrators
{
    using System;
    using System.Runtime.Remoting.Channels;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Interop;
    using Core.Config;
    using Core.Extensions;
    using Core.Orchestrators;
    using Core.ViewModel;
    using Gma.System.MouseKeyHook;
    using Utility;

    public class Translator : ITranslator
    {
        private readonly GrowlNotifiactions growlNotifications;
        private readonly MainWindow mainWindow;
        private readonly IStartupConfiguration startupConfiguration;
        private readonly IKeyboardMouseEvents globalMouseHook;
        private IntPtr hWndNextViewer;
        private HwndSource hWndSource;
        private bool isMouseDown;

        public Translator(MainWindow mainWindow, GrowlNotifiactions growlNotifications, IStartupConfiguration startupConfiguration)
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
            this.globalMouseHook = Hook.GlobalEvents();
        }

        public bool IsInitialized { get; set; }

        public void Initialize()
        {
            var wih = new WindowInteropHelper(this.mainWindow);
            this.hWndSource = HwndSource.FromHwnd(wih.Handle);
            var source = this.hWndSource;
            if (source != null)
            {
                source.AddHook(this.WinProc); // start processing window messages
                this.hWndNextViewer = Win32.SetClipboardViewer(source.Handle); // set this window as a viewer
            }
            this.IsInitialized = true;
            this.globalMouseHook.MouseDoubleClick += this.MouseDoubleClicked;
            this.globalMouseHook.MouseDown += this.MouseDown;
            this.globalMouseHook.MouseUp += this.MouseUp;
            this.growlNotifications.OnDispose += ClearAllNotifications;
            this.growlNotifications.Top = SystemParameters.WorkArea.Top + this.startupConfiguration.TopOffset;
            this.growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - this.startupConfiguration.LeftOffset;
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            if (this.isMouseDown)
            {
                SendKeys.SendWait("^c");
                this.isMouseDown = false;
            }
            this.isMouseDown = false;
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            this.isMouseDown = true;
        }

        private void MouseDoubleClicked(object sender, MouseEventArgs e)
        {
            SendKeys.SendWait("^c");
        }

        public void Dispose()
        {
            Win32.ChangeClipboardChain(this.hWndSource.Handle, this.hWndNextViewer);
            this.hWndNextViewer = IntPtr.Zero;
            this.hWndSource.RemoveHook(this.WinProc);
            this.IsInitialized = false;
            this.growlNotifications.OnDispose -= ClearAllNotifications;
        }

        public event EventHandler WhenClipboardContainsTextEventHandler;

        public event EventHandler<WhenNotificationAddEventArgs> WhenNotificationAddEventHandler;

        public void AddNotification(string title, string imageUrl, string message)
        {
            this.growlNotifications.AddNotificationSync(new Notification
            {
                ImageUrl = imageUrl,
                Title = title,
                Message = message
            });
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




        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WmChangecbchain:
                    if (wParam == this.hWndNextViewer)
                    {
                        this.hWndNextViewer = lParam; //clipboard viewer chain changed, need to fix it.
                    }
                    else if (this.hWndNextViewer != IntPtr.Zero)
                    {
                        Win32.SendMessage(this.hWndNextViewer, msg, wParam, lParam); //pass the message to the next viewer.
                    }

                    break;
                case Win32.WmDrawclipboard:
                    this.WhenClipboardContainsTextEventHandler.InvokeSafely(this, EventArgs.Empty); //clipboard content changed
                    Win32.SendMessage(this.hWndNextViewer, msg, wParam, lParam); //pass the message to the next viewer
                    break;
            }

            return IntPtr.Zero;
        }
    }
}