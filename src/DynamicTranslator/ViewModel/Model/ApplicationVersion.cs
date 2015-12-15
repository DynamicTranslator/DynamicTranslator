namespace DynamicTranslator.ViewModel.Model
{
    public static class ApplicationVersion
    {
        public const string Version100 = "1.0.0";
        public const string Version200 = "2.0.0";
        public const string Version310 = "3.1.0";
        public const string Version320 = "3.2.0";

        public static string GetCurrentVersion()
        {
            return Version320;
        }
    }
}