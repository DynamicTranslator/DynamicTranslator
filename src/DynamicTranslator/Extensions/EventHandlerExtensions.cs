using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DynamicTranslator.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void InvokeSafely(this PropertyChangedEventHandler eventHandler, object sender)
        {
            eventHandler.InvokeSafely(sender, (PropertyChangedEventArgs)EventArgs.Empty);
        }

        public static void InvokeSafely(this PropertyChangedEventHandler eventHandler, object sender, PropertyChangedEventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }

        public static void InvokeSafely(this EventHandler eventHandler, object sender)
        {
            eventHandler.InvokeSafely(sender, EventArgs.Empty);
        }

        public static void InvokeSafely(this EventHandler eventHandler, object sender, EventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }

        public static void InvokeSafely<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e)
            where TEventArgs : EventArgs
        {
            eventHandler?.Invoke(sender, e);
        }

        public static Task InvokeSafelyAsync<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e)
            where TEventArgs : EventArgs
        {
            return Task.Run(() => eventHandler?.Invoke(sender, e));
        }
    }
}
