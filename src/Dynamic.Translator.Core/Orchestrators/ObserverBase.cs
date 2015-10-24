namespace Dynamic.Translator.Core.Orchestrators
{
    using System;

    public class ObserverBase<T> : IObserver<T>
    {
        public virtual void OnNext(T value)
        {
        }

        public virtual void OnError(Exception error)
        {
        }

        public virtual void OnCompleted()
        {
        }
    }
}