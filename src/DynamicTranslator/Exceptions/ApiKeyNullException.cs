using System;

namespace DynamicTranslator.Exceptions
{
    public class ApiKeyNullException : Exception
    {
        public ApiKeyNullException(string message, Exception ex, object[] messageParameters) : base(message, ex)
        {
            MessageParameters = messageParameters;
        }

        public ApiKeyNullException(string message, Exception ex) : base(message, ex) {}

        public ApiKeyNullException(string message) : base(message) {}

        public object[] MessageParameters { get; set; }

        public string ResultMessage { get; set; }
    }
}
