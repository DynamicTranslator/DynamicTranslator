namespace Dynamic.Translator.Core.Exception
{
    using System;

    public class MaximumCharacterLimitException : Exception
    {
        public MaximumCharacterLimitException(string message, Exception ex, object[] messageParameters) : base(message, ex)
        {
            this.MessageParameters = messageParameters;
        }

        public MaximumCharacterLimitException(string message, Exception ex) : base(message, ex)
        {
        }

        public MaximumCharacterLimitException(string message) : base(message)
        {
        }

        public string ResultMessage { get; set; }

        public object[] MessageParameters { get; set; }
    }
}