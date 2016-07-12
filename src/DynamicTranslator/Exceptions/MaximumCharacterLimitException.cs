namespace DynamicTranslator.Exceptions
{
    public class MaximumCharacterLimitException : System.Exception
    {
        public object[] MessageParameters { get; set; }

        public string ResultMessage { get; set; }

        public MaximumCharacterLimitException(string message, System.Exception ex, object[] messageParameters) : base(message, ex)
        {
            MessageParameters = messageParameters;
        }

        public MaximumCharacterLimitException(string message, System.Exception ex) : base(message, ex) {}

        public MaximumCharacterLimitException(string message) : base(message) {}
    }
}