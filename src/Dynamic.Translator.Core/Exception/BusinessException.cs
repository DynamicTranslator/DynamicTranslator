namespace DynamicTranslator.Core.Exception
{
    #region using

    using System;

    #endregion

    public class BusinessException : Exception
    {
        public BusinessException(string message, Exception ex, object[] messageParameters) : base(message, ex)
        {
            MessageParameters = messageParameters;
        }

        public BusinessException(string message, Exception ex) : base(message, ex)
        {
        }

        public BusinessException(string message) : base(message)
        {
        }

        public string ResultMessage { get; set; }

        public object[] MessageParameters { get; set; }
    }
}