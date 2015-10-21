namespace Dynamic.Tureng.Translator.Orchestrator.Observable
{
    using System.Windows;

    public class Translator : ITranslator
    {
        public Maybe<string> GetClipBoardState()
        {
            return Clipboard.ContainsText() ? new Maybe<string>(Clipboard.GetText()) : new Maybe<string>();
        }
    }

    public enum TextStatus
    {
        CLIPBOARD_NO_TEXT = 0,
        CLIPBOARD_HAS_TEXT = 1
    }
}