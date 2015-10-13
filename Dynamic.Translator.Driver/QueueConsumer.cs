namespace Dynamic.Translator.Driver
{
    using System;
    using System.IO;

    public class QueueConsumer
    {
        readonly private IObserver<Stream> observer;
        readonly private IQueue queue;

        public QueueConsumer(IQueue queue, IObserver<Stream> observer)
        {
            this.queue = queue;
            this.observer = observer;
        }

        public void ConsumeAll()
        {
            foreach (var s in this.queue)
            {
                try
                {
                    this.observer.OnNext(s);
                    this.queue.Delete(s);
                }
                catch (Exception e)
                {
                    if (e.IsUnsafeToSuppress())
                        throw;
                }
            }
        }
    }
}