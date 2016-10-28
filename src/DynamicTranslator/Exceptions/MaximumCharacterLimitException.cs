using System;

namespace DynamicTranslator.Exceptions
{
    public class MaximumCharacterLimitException : Exception
    {
        public MaximumCharacterLimitException(string message, Exception ex, object[] messageParameters) : base(message, ex)
        {
            MessageParameters = messageParameters;
        }

        public MaximumCharacterLimitException(string message, Exception ex) : base(message, ex) {}

        public MaximumCharacterLimitException(string message) : base(message) {}

        public object[] MessageParameters { get; set; }
    }
}
