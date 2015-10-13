namespace Dynamic.Translator.Driver
{
    using System;

    public class EventDispatcher<T> : IObserver<object>
    {
        readonly private IEventHandler<T> eventHandler;

        public EventDispatcher(IEventHandler<T> eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(object value)
        {
            if (value is T)
                this.eventHandler.Handle((T)value);
        }
    }
}