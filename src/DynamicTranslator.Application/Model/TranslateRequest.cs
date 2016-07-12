namespace DynamicTranslator.Application.Model
{
    public class TranslateRequest
    {
        public string CurrentText { get; protected set; }

        public string FromLanguageExtension { get; protected set; }

        public TranslateRequest(string currentText, string fromLanguageExtension)
        {
            CurrentText = currentText;
            FromLanguageExtension = fromLanguageExtension;
        }
    }
}