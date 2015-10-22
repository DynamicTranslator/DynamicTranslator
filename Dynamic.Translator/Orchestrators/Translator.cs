namespace Dynamic.Tureng.Translator.Orchestrators
{
    using System;
    using System.Windows.Interop;
    using Dynamic.Translator.Core.Extensions;
    using Dynamic.Translator.Core.Orchestrators;
    using Utility;

    public class Translator : ITranslator
    {
        private readonly MainWindow mainWindow;
        private IntPtr hWndNextViewer;
        private HwndSource hWndSource;

        public Translator(MainWindow mainWindow)
        {
            if (mainWindow == null)
                throw new ArgumentNullException(nameof(mainWindow));

            this.mainWindow = mainWindow;
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
        }

        public void Dispose()
        {
            Win32.ChangeClipboardChain(this.hWndSource.Handle, this.hWndNextViewer);
            this.hWndNextViewer = IntPtr.Zero;
            this.hWndSource.RemoveHook(this.WinProc);
            this.IsInitialized = false;
        }

        public void AddNotificationEvent(object sender, WhenNotificationAddEventArgs eventArgs)
        {
            this.WhenNotificationAddEventHandler.InvokeSafely(this, eventArgs);
        }

        public event EventHandler WhenClipboardContainsTextEventHandler;

        public event EventHandler<WhenNotificationAddEventArgs> WhenNotificationAddEventHandler;


        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WmChangecbchain:
                    if (wParam == this.hWndNextViewer)
                    {
                        // clipboard viewer chain changed, need to fix it.
                        this.hWndNextViewer = lParam;
                    }
                    else if (this.hWndNextViewer != IntPtr.Zero)
                    {
                        // pass the message to the next viewer.
                        Win32.SendMessage(this.hWndNextViewer, msg, wParam, lParam);
                    }

                    break;
                case Win32.WmDrawclipboard:

                    // clipboard content changed
                    this.WhenClipboardContainsTextEventHandler.InvokeSafely(this, EventArgs.Empty);

                    // pass the message to the next viewer.
                    Win32.SendMessage(this.hWndNextViewer, msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }
    }
}