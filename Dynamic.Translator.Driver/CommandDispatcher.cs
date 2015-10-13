namespace Dynamic.Translator.Driver
{
    using System;

    public class CommandDispatcher<T> : IObserver<object>
    {
        readonly private ICommandHandler<T> commandHandler;

        public CommandDispatcher(ICommandHandler<T> commandHandler)
        {
            this.commandHandler = commandHandler;
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
                this.commandHandler.Execute((T)value);
        }
    }
}