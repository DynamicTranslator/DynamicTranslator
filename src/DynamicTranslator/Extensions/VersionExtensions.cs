using System;

namespace DynamicTranslator.Extensions
{
    public static class VersionExtensions
    {
        public static Version IncrementMinor(this Version @this)
        {
            return new Version(@this.Major, @this.Minor + 1, @this.Build);
        }

        public static Version IncrementMajor(this Version @this)
        {
            return new Version(@this.Major + 1, @this.Minor, @this.Build);
        }

        public static Version IncrementBuild(this Version @this)
        {
            return new Version(@this.Major, @this.Minor, @this.Build + 1);
        }
    }
}