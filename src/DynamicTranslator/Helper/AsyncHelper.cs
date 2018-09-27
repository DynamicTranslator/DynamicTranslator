using System.Reflection;
using System.Threading.Tasks;

namespace DynamicTranslator.Helper
{
    public static class AsyncHelper
    {
        /// <summary>
        ///     Checks if given method is an async method.
        /// </summary>
        /// <param name="method">A method to check</param>
        public static bool IsAsyncMethod(MethodInfo method)
        {
            return method.ReturnType == typeof(Task) || method.ReturnType.IsGenericType &&
                   method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }
    }
}