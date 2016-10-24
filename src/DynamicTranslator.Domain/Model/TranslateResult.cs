using System;
using System.Runtime.Serialization;

namespace DynamicTranslator.Domain.Model
{
    [Serializable]
    public class TranslateResult
    {
        public TranslateResult() : this(true, new Maybe<string>()) {}

        public TranslateResult(bool isSucess, Maybe<string> resultMessage)
        {
            IsSuccess = isSucess;
            ResultMessage = resultMessage;
        }

        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public Maybe<string> ResultMessage { get; set; }
    }
}
