using System;
using System.Windows;
using System.Windows.Threading;

namespace DynamicTranslator.Extensions
{
    public static class MultiThreadUiExtensions
    {
        public static DispatcherOperation DispatchingAsync(this Window obj, Action action)
        {
            return obj.Dispatcher.InvokeAsync(action);
        }
    }
}
