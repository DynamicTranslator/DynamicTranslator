namespace Dynamic.Translator.Core.Exception
{
    #region using

    using System;

    #endregion

    public class MaximumCharacterLimitException : Exception
    {
        public MaximumCharacterLimitException(string message, Exception ex, object[] messageParameters) : base(message, ex)
        {
            MessageParameters = messageParameters;
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