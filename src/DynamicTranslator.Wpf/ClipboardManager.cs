using System;
using System.Windows;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Wpf
{
    public class ClipboardManager
    {
        public void Clear()
        {
            Clipboard.Clear();
        }

        public string GetCurrentText()
        {
            return Clipboard.GetText().RemoveSpecialCharacters().ToLowerInvariant();
        }

        public bool ContainsText()
        {
            try
            {
                return Clipboard.ContainsText() && !string.IsNullOrEmpty(Clipboard.GetText().Trim());
            }
            catch (Exception)
            {
                return ContainsText();
            }
           
        }
    }
}
