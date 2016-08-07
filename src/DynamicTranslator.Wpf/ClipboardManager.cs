using System.Windows;

using Abp.Dependency;

using DynamicTranslator.Extensions;

namespace DynamicTranslator.Wpf
{
    public class ClipboardManager : IClipboardManager, ITransientDependency
    {
        public void Clear()
        {
            Clipboard.Clear();
        }

        public string GetCurrentText()
        {
            return Clipboard.GetText().RemoveSpecialCharacters().ToLowerInvariant();
        }

        public bool IsContainsText()
        {
            return Clipboard.ContainsText() && !string.IsNullOrEmpty(Clipboard.GetText().Trim());
        }
    }
}