using System;

namespace DynamicTranslator.Exceptions
{
    public class MaximumCharacterLimitException : Exception
    {
        public MaximumCharacterLimitException(string message) : base(message) {}
    }
}
