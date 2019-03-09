namespace DynamicTranslator
{
    public class TranslateRequest
    {
        public TranslateRequest(string currentText, string fromLanguageExtension)
        {
            CurrentText = currentText;
            FromLanguageExtension = fromLanguageExtension;
        }

        public string CurrentText { get; protected set; }

        public string FromLanguageExtension { get; protected set; }
    }
}