using System;

namespace DynamicTranslator.Exceptions
{
    public class ApiKeyNullException : Exception
    {
        public ApiKeyNullException(string message) : base(message) {}
    }
}
