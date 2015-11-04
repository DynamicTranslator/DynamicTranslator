namespace Dynamic.Translator.Core.Extensions
{
    #region using

    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    #endregion

    public static class EventHandlerExtensions
    {
        /// <summary>
        ///     Raises given event safely with given arguments.
        /// </summary>
        /// <param name="eventHandler">The event handler</param>
        /// <param name="sender">Source of the event</param>
        public static void InvokeSafely(this PropertyChangedEventHandler eventHandler, object sender)
        {
            eventHandler.InvokeSafely(sender, (PropertyChangedEventArgs) EventArgs.Empty);
        }

        /// <summary>
        ///     Raises given event safely with given arguments.
        /// </summary>
        /// <param name="eventHandler">The event handler</param>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event argument</param>
        public static void InvokeSafely(this PropertyChangedEventHandler eventHandler, object sender, PropertyChangedEventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }

        public static void InvokeSafely(this EventHandler eventHandler, object sender)
        {
            eventHandler.InvokeSafely(sender, EventArgs.Empty);
        }

        /// <summary>
        ///     Raises given event safely with given arguments.
        /// </summary>
        /// <param name="eventHandler">The event handler</param>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event argument</param>
        public static void InvokeSafely(this EventHandler eventHandler, object sender, EventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }

        /// <summary>
        ///     Raises given event safely with given arguments.
        /// </summary>
        /// <typeparam name="TEventArgs">Type of the <see cref="EventArgs" /></typeparam>
        /// <param name="eventHandler">The event handler</param>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event argument</param>
        public static void InvokeSafely<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e)
            where TEventArgs : EventArgs
        {
            eventHandler?.Invoke(sender, e);
        }

        public static async Task InvokeSafelyAsync<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e)
            where TEventArgs : EventArgs
        {
            await Task.Run(() => eventHandler?.Invoke(sender, e));
        }
    }
}