namespace DynamicTranslator
{
    public static class ApplicationVersion
    {
        public const string CurrentVersion = "3.3.0";

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