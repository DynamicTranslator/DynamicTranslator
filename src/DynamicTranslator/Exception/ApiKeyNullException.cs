namespace DynamicTranslator.Exception
{
    #region using

    

    #endregion

    public class ApiKeyNullException : System.Exception
    {
        public ApiKeyNullException(string message, System.Exception ex, object[] messageParameters) : base(message, ex)
        {
            MessageParameters = messageParameters;
        }

        public ApiKeyNullException(string message, System.Exception ex) : base(message, ex) {}

        public ApiKeyNullException(string message) : base(message) {}

        public object[] MessageParameters { get; set; }

        public string ResultMessage { get; set; }
    }
}