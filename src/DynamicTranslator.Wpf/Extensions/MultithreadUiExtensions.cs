using System;
using System.Windows;
using System.Windows.Threading;

namespace DynamicTranslator.Wpf.Extensions
{
    public static class MultithreadUiExtensions
    {
        public static DispatcherOperation DispatchingAsync(this Window obj, Action action)
        {
            return obj.Dispatcher.InvokeAsync(action);
        }
    }
}
