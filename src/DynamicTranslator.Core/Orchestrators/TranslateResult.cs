namespace DynamicTranslator.Core.Orchestrators
{
    public class TranslateResult
    {
        public TranslateResult() : this(true, new Maybe<string>())
        {
        }

        public TranslateResult(bool isSucess, Maybe<string> resultMessage)
        {
            IsSucess = isSucess;
            ResultMessage = resultMessage;
        }

        public bool IsSucess { get; set; }

        public Maybe<string> ResultMessage { get; set; }
    }
}