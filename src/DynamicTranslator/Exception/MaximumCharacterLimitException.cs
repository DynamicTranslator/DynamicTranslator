namespace DynamicTranslator.Exception
{
    public class MaximumCharacterLimitException : System.Exception
    {
        public MaximumCharacterLimitException(string message, System.Exception ex, object[] messageParameters) : base(message, ex)
        {
            MessageParameters = messageParameters;
        }

        public MaximumCharacterLimitException(string message, System.Exception ex) : base(message, ex) {}

        public MaximumCharacterLimitException(string message) : base(message) {}

        public object[] MessageParameters { get; set; }

        public string ResultMessage { get; set; }
    }
}