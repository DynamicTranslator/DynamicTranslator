using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DynamicTranslator.Helper
{
    #region using

    

    #endregion

    public static class ExtendedAsyncHelper
    {
        public static async Task AwaitTaskWithFinally(Task actualReturnValue, Action<System.Exception> finalAction)
        {
            System.Exception exception = null;

            try
            {
                await actualReturnValue;
            }
            catch (System.Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        public static async Task<T> AwaitTaskWithFinallyAndGetResult<T>(Task<T> actualReturnValue, Action<System.Exception> finalAction)
        {
            System.Exception exception = null;

            try
            {
                return await actualReturnValue;
            }
            catch (System.Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        public static async Task AwaitTaskWithPostActionAndFinally(Task actualReturnValue, Func<Task> postAction, Action<System.Exception> finalAction)
        {
            System.Exception exception = null;

            try
            {
                await actualReturnValue;
                await postAction();
            }
            catch (System.Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue, Func<Task> postAction, Action<System.Exception> finalAction)
        {
            System.Exception exception = null;

            try
            {
                var result = await actualReturnValue;
                await postAction();
                return result;
            }
            catch (System.Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        public static async Task AwaitTaskWithPreActionAndPostActionAndFinally(Func<Task> actualReturnValue, Func<Task> preAction = null, Func<Task> postAction = null,
            Action<System.Exception> finalAction = null)
        {
            System.Exception exception = null;

            try
            {
                if (preAction != null)
                {
                    await preAction();
                }

                await actualReturnValue();

                if (postAction != null)
                {
                    await postAction();
                }
            }
            catch (System.Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                if (finalAction != null)
                {
                    finalAction(exception);
                }
            }
        }

        public static async Task<T> AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult<T>(Func<Task<T>> actualReturnValue, Func<Task> preAction = null,
            Func<Task> postAction = null, Action<System.Exception> finalAction = null)
        {
            System.Exception exception = null;

            try
            {
                if (preAction != null)
                {
                    await preAction();
                }

                var result = await actualReturnValue();

                if (postAction != null)
                {
                    await postAction();
                }

                return result;
            }
            catch (System.Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                if (finalAction != null)
                {
                    finalAction(exception);
                }
            }
        }

        public static object CallAwaitTaskWithFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Action<System.Exception> finalAction)
        {
            return typeof(ExtendedAsyncHelper)
                .GetMethod("AwaitTaskWithFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new[] {actualReturnValue, finalAction});
        }

        public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Func<Task> action, Action<System.Exception> finalAction)
        {
            return typeof(ExtendedAsyncHelper)
                .GetMethod("AwaitTaskWithPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new[] {actualReturnValue, action, finalAction});
        }

        public static object CallAwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult(Type taskReturnType, Func<object> actualReturnValue, Func<Task> preAction = null,
            Func<Task> postAction = null, Action<System.Exception> finalAction = null)
        {
            return typeof(ExtendedAsyncHelper)
                .GetMethod("AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] {actualReturnValue, preAction, postAction, finalAction});
        }
    }
}