using System;

namespace DynamicTranslator.Exceptions
{
    public class NotSupportedLanguageException : Exception
    {
        public NotSupportedLanguageException(string message, Exception ex, params object[] messageParameters) : base(message, ex)
        {
            MessageParameters = messageParameters;
        }

        public NotSupportedLanguageException(string message, Exception ex) : base(message, ex) {}

        public NotSupportedLanguageException(string message) : base(message) {}

        public object[] MessageParameters { get; set; }
    }
}
