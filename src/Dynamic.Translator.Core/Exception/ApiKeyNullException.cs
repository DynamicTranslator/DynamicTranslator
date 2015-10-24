namespace Dynamic.Translator.Core.Exception
{
    using System;

    public class ApiKeyNullException : Exception
    {
        public ApiKeyNullException(string message, Exception ex, object[] messageParameters) : base(message, ex)
        {
            this.MessageParameters = messageParameters;
        }

        public ApiKeyNullException(string message, Exception ex) : base(message, ex)
        {
        }

        public ApiKeyNullException(string message) : base(message)
        {
        }

        public string ResultMessage { get; set; }

        public object[] MessageParameters { get; set; }
    }
}