using System;
using System.Runtime.InteropServices;

namespace DynamicTranslator.Utility
{
    #region using

    

    #endregion

    /// <summary>
    ///     This static class holds the Win32 function declarations and constants needed by
    ///     this sample application.
    /// </summary>
    internal static class Win32
    {
        /// <summary>
        ///     A clipboard viewer window receives the WM_CHANGECBCHAIN message when
        ///     another window is removing itself from the clipboard viewer chain.
        /// </summary>
        internal const int WmChangecbchain = 0x030D;

        /// <summary>
        ///     The WM_DRAWCLIPBOARD message notifies a clipboard viewer window that
        ///     the content of the clipboard has changed.
        /// </summary>
        internal const int WmDrawclipboard = 0x0308;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
    }
}