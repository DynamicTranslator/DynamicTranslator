namespace DynamicTranslator.Core.Exception
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

        public object[] MessageParameters { get; set; }

        public string ResultMessage { get; set; }
    }
}