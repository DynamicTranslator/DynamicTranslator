namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using System;

    #endregion

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