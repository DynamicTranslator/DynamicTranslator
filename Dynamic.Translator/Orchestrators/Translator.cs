namespace Dynamic.Tureng.Translator.Orchestrators
{
    using System;
    using System.Windows.Interop;
    using Dynamic.Translator.Core.Extensions;
    using Dynamic.Translator.Core.Orchestrators;
    using Observables;
    using Utility;

    public class Translator : ITranslator
    {
        private IntPtr hWndNextViewer;

        public Translator(MainWindow mainWindow)
        {
            if (mainWindow == null)
                throw new ArgumentNullException(nameof(mainWindow));

            var wih = new WindowInteropHelper(mainWindow);
            this.HWndSource = HwndSource.FromHwnd(wih.Handle);

            var source = this.HWndSource;
            if (source != null)
            {
                source.AddHook(this.WinProc); // start processing window messages
                this.hWndNextViewer = Win32.SetClipboardViewer(source.Handle); // set this window as a viewer
            }
        }

        public HwndSource HWndSource { get; }

        public void Initialize()
        {
        }

        public void WhenNotificationAddEventInvoker(object sender, WhenNotificationAddEventArgs eventArgs)
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