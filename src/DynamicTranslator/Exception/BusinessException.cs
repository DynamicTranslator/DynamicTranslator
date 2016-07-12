namespace DynamicTranslator.Exception
{
    public class BusinessException : System.Exception
    {
        public object[] MessageParameters { get; set; }

        public string ResultMessage { get; set; }

        public BusinessException(string message, System.Exception ex, object[] messageParameters) : base(message, ex)
        {
            MessageParameters = messageParameters;
        }

        public BusinessException(string message, System.Exception ex) : base(message, ex) {}

        public BusinessException(string message) : base(message) {}
    }
}