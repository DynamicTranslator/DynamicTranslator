using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DynamicTranslator.Helper
{
    public static class ExtendedAsyncHelper
    {
        public static async Task<T> AwaitTaskWithFinallyAndGetResult<T>(Task<T> actualReturnValue, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                return await actualReturnValue;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        public static object CallAwaitTaskWithFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Action<Exception> finalAction)
            => typeof(ExtendedAsyncHelper)
               .GetMethod(nameof(AwaitTaskWithFinallyAndGetResult), BindingFlags.Public | BindingFlags.Static)
               ?.MakeGenericMethod(taskReturnType)
               .Invoke(null, new[] { actualReturnValue, finalAction });
    }
}
