namespace DynamicTranslator.Core
{
    public interface IClipboardManager
    {
        void Clear();
        string GetCurrentText();
        bool ContainsText();
    }
}