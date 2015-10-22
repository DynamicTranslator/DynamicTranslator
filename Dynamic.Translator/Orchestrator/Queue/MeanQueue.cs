namespace Dynamic.Tureng.Translator.Orchestrator.Queue
{
    using System.Collections;
    using System.Collections.Generic;

    public class MeanQueue : IQueue
    {
        public IEnumerator<string> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string text)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}