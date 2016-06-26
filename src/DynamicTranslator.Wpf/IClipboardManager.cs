namespace DynamicTranslator.Wpf
{
    public interface IClipboardManager
    {
        string GetCurrentText();

        bool IsContainsText();
    }
}