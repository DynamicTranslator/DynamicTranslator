namespace Dynamic.Translator.Driver
{
    using System;

    public static class ExceptionPolicy
    {
        public static bool IsUnsafeToSuppress(this Exception e)
        {
            // Cheating by suppressing every caught exception. Don't do this in production code. 
            return false;
        }
    }
}