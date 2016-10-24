namespace DynamicTranslator
{
    public static class ApplicationVersion
    {
        public const string CurrentVersion = "3.4.1";

        public static string GetCurrentVersion()
        {
            return CurrentVersion;
        }

        public static bool Is(string version)
        {
            return CurrentVersion == version;
        }
    }
}
