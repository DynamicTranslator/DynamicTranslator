namespace DynamicTranslator
{
    using System;
    using System.Windows;
    using Core;
    using Core.Extensions;

    public class ClipboardManager : IClipboardManager
    {
        public void Clear()
        {
            Clipboard.Clear();
        }

        public string GetCurrentText()
        {
            try
            {
                return ContainsText()
                    ? Clipboard.GetText().RemoveSpecialCharacters().ToLowerInvariant()
                    : string.Empty;
            }
            catch (Exception) { return string.Empty; }
        }

        static bool ContainsText()
        {
            static bool Retry(int times = 1)
            {
                try { return Clipboard.ContainsText() && !string.IsNullOrEmpty(Clipboard.GetText().Trim()); }
                catch (Exception) when (times < 3) { return Retry(times + 1); }
                catch (Exception) { return false; }
            }

            return Retry();
        }
    }
}