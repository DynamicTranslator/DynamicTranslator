namespace DynamicTranslator.Model
{
    public class TranslateResult
    {
        public TranslateResult() : this(true, string.Empty)
        {
        }

        public TranslateResult(bool isSuccess, string result)
        {
            IsSuccess = isSuccess;
            ResultMessage = result;
        }

        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
    }
}